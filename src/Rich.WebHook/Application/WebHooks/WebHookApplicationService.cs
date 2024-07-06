using System.Data;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Ganss.Xss;
using Microsoft.Extensions.DependencyInjection.Exceptions;
using Microsoft.IdentityModel.Tokens;
using Rich.WebHook.Application.Users;
using Rich.WebHook.Application.WebHooks.Dto;
using Rich.WebHook.Common.MQ;
using Rich.WebHook.Dmain.Shared.MessageForward;
using Rich.WebHook.EntityFramework.Model;
using Rich.WebHook.Repository.WebHookReceivers;
using Rich.WebHook.Repository.Webhooks;
using Scriban;

namespace Rich.WebHook.Application.WebHooks;

public class WebHookApplicationService(
    IHttpContextAccessor httpContextAccessor,
    IRabbitMqService rabbitMqService,
    IRichSession richSession,
    IWebhookRepository webhookRepository,
    IWebHookReceiverRepository webHookReceiverRepository)
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

        var dataStr = Convert.ToString(data);

        // 模板渲染字段严格区分大小写
        var template = webHookDetail.TemplateText.IsNullOrEmpty()
            ? null
            : Template.Parse(webHookDetail.TemplateText);
        var result = template == null
            ? dataStr
            : await template.RenderAsync(data);

        if (webHookDetail.Receivers == null)
        {
            return string.Empty;
        }

        if (webHookDetail.Receivers.Any(x => x.Client == ReceiveClientEnum.WeChat))
        {
            var receiverItems = webHookDetail.Receivers
                .Where(r => r.Client == ReceiveClientEnum.WeChat);

            foreach (var receiver in receiverItems)
            {
                var pushData = new
                {
                    MemberType = receiver.MemberType,
                    To = receiver.Receivers,
                    Title = title,
                    Content = result
                };
                rabbitMqService.PublishMessage("webhook_wechat", "webhook_wechat_queue",
                    JsonSerializer.Serialize(pushData));
            }
        }

        if (webHookDetail.Receivers.Any(x => x.Client == ReceiveClientEnum.Mq))
        {
            var receiverMq = webHookDetail.Receivers.Where(x => x.Client == ReceiveClientEnum.Mq);
            foreach (var receiverItem in receiverMq)
            {
                // var pushData = new
                // {
                //     Title = title,
                //     Content = result
                // };
                foreach (var receiver in receiverItem.Receivers)
                {
                    // rabbitMqService.PublishMessage("webhook_mq", receiver,
                    //     JsonSerializer.Serialize(pushData));
                    rabbitMqService.PublishMessage("webhook_mq", receiver, result);
                }
            }
        }

        if (webHookDetail.Receivers.Any(x => x.Client == ReceiveClientEnum.Http))
        {
            var receiverHttp = webHookDetail.Receivers.Where(x => x.Client == ReceiveClientEnum.Http);
            foreach (var receiverItem in receiverHttp)
            {
                var content = new StringContent(result, Encoding.UTF8, "application/json");
                foreach (var receiver in receiverItem.Receivers)
                {
                    using var client = new HttpClient();
                    var response = await client.PostAsync(receiver, content);
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Response: " + responseBody);
                }
            }
        }

        return result;
    }

    public async Task CreateAsync(CreateWebHookDto input)
    {
        var sanitizer = new HtmlSanitizer();
        var templateText = sanitizer.Sanitize(input.TemplateText);

        var encodedTemplateText = HtmlEncoder.Default.Encode(templateText);
        var template = Template.Parse(encodedTemplateText);
        if (template.HasErrors)
        {
            throw new UserFriendException(HttpStatusCode.Forbidden, "模板格式有误!");
        }

        var webHook = new WebHookSetting()
        {
            UserId = richSession.GetUserId(),
            Title = input.Title,
            TemplateText = templateText,
            Remark = input.Remark,
            Token = $"{Guid.NewGuid():N}",
            CreateTime = DateTime.Now
        };
        webHook = await webhookRepository.AddAsync(webHook);

        var receivers = new List<WebHookReceiver>();
        foreach (var item in input.Receivers)
        {
            receivers.Add(new WebHookReceiver(webHook.Id, item.Client, item.Receivers, item.MemberType));
        }

        await webHookReceiverRepository.BatchAsync(receivers);
    }

    public async Task<WebHookDetailDto?> GetAsync(int id)
    {
        var userId = richSession.UserId.Value;
        var webHook = await webhookRepository.GetAsync(userId, id);
        var receivers = await webHookReceiverRepository.GetReceiversByWebHookId(id);
        var outPut = FillWebHookDetail(webHook, receivers);

        return outPut;
    }

    private WebHookDetailDto FillWebHookDetail(WebHookSetting? webHook, IEnumerable<WebHookReceiver> receivers)
    {
        if (webHook is null) throw new Exception("数据不存在");
        var httpContext = httpContextAccessor.HttpContext;
        var host = $"{httpContext?.Request.Scheme}://{httpContext?.Request.Host}";
        var webHookUrl = $"{host}/api/webhook/{webHook.Token}";

        var outPut = new WebHookDetailDto
        {
            Id = webHook.Id,
            Title = webHook.Title,
            TemplateText = webHook.TemplateText,
            Url = webHookUrl,
            Remark = webHook.Remark,
            Receivers = receivers?.Select(x =>
                new WebHookReceiverDto()
                {
                    Client = x.Client,
                    MemberType = x.MemberType,
                    Receivers = x.Receivers
                }).ToList()
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

        var receivers = await webHookReceiverRepository.GetReceiversByWebHookId(webHook.Id);
        var outPut = FillWebHookDetail(webHook, receivers);
        return outPut;
    }

    public async Task<IEnumerable<WebHookDetailDto>?> GetListAsync()
    {
        var userId = richSession.GetUserId();

        // 使用 EF Core 的 Include 方法预先加载相关的 WebHookReceivers
        var webHooksWithReceivers = await webhookRepository.GetListWithReceiversAsync(userId);

        // 使用 LINQ 一次性处理所有 WebHook 和它们的 Receivers
        var result = webHooksWithReceivers?.Select(webHook =>
        {
            var webhookReceivers = webHook.Receivers.ToList();
            var detail = FillWebHookDetail(webHook, webhookReceivers);
            return detail;
        }).ToList();

        return result;
    }

    public async Task DeleteAsync(int id)
    {
        await webhookRepository.Delete(id);
        await webHookReceiverRepository.DeleteAsync(id);
    }
}