using System.Data;
using System.Text.Json;
using Rich.WebHook.Application.Users;
using Rich.WebHook.Application.WebHooks.Dto;
using Rich.WebHook.Common.MQ;
using Rich.WebHook.EntityFramework.Model;
using Rich.WebHook.Repository.Templates;
using Rich.WebHook.Repository.Webhooks;
using Scriban;

namespace Rich.WebHook.Application.WebHooks;

public class WebHookApplicationService(
    IHttpContextAccessor httpContextAccessor,
    IRabbitMqService rabbitMqService,
    IRichSession richSession,
    IWebhookRepository webhookRepository,
    ITemplateRepository templateRepository)
    : IWebHookApplicationService
{
    /// <summary>
    /// WebHook 通知处理
    /// </summary>
    /// <param name="token"></param>
    /// <param name="title"></param>
    /// <param name="data"></param>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<string?> ReceiveDataAsync(string token, string? title, object data)
    {
        var webHookDetail = await GetByTokenAsync(token);
        if (webHookDetail is null)
            throw new Exception("token无效");

        // 模板渲染字段严格区分大小写
        var template = Template.Parse(webHookDetail.TemplateText);
        var result = await template.RenderAsync(data);

        var pushData = new
        {
            Type = 0,
            To = new List<string> { "大狗" },
            Title = title,
            Content = result
        };
        var jsonOptions = new JsonSerializerOptions()
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        rabbitMqService.PublishMessage("webhook_chat", "webhook_chat_queue",
            JsonSerializer.Serialize(pushData, jsonOptions));

        return result;
    }

    public async Task<WebHookSetting?> CreateAsync(CreateWebHookDto input)
    {
        var webHook = new WebHookSetting()
        {
            UserId = richSession.UserId.Value,
            TemplateId = input.TemplateId,
            Remark = input.Remark,
            Token = $"{Guid.NewGuid():N}",
            CreateTime = DateTime.Now
        };
        webHook = await webhookRepository.AddAsync(webHook);
        return webHook;
    }

    public async Task<WebHookDetailDto?> GetAsync(int id)
    {
        var webHook = await webhookRepository.GetAsync(id);
        var outPut = await FillWebHookDetail(webHook);

        return outPut;
    }

    private async Task<WebHookDetailDto> FillWebHookDetail(WebHookSetting? webHook)
    {
        if (webHook is null) throw new Exception("数据不存在");
        var template = await templateRepository.GetAsync(webHook.TemplateId);
        if (template is null) throw new Exception("未找到模板数据");
        var filePath = Path.Combine(".", "Data", "WebHookTemplates", template.FileName);
        var templateText = await File.ReadAllTextAsync(filePath);

        var httpContext = httpContextAccessor.HttpContext;
        var host = $"{httpContext?.Request.Scheme}://{httpContext?.Request.Host}";
        var webHookUrl = $"{host}/api/webhook/{webHook.Token}";
        // webHook.Id, templateText, webHook.Remark, webHookUrl
        var outPut = new WebHookDetailDto
        {
            Id = webHook.Id,
            TemplateText = templateText,
            Url = webHookUrl,
            Remark = webHook.Remark
        };
        return outPut;
    }

    public async Task<WebHookDetailDto?> GetByTokenAsync(string token)
    {
        var webHook = await webhookRepository.GetByTokenAsync(token);
        if (webHook is null)
        {
            throw new DataException("非法请求");
        }

        var outPut = await FillWebHookDetail(webHook);
        return outPut;
    }

    public async Task UpdateTemplateAsync(int id, int templateId)
    {
        await webhookRepository.UpdateTemplate(id, templateId);
    }

    public async Task DeleteAsync(int id)
    {
        await webhookRepository.Delete(id);
    }
}