using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Rich.WebHook.Application.WebHooks;
using Rich.WebHook.Application.WebHooks.Dto;
using Scriban;

namespace Rich.WebHook.Apis;

/// <summary>
/// WebHook 接口
/// </summary>
public static class WebHookApi
{
    public static RouteGroupBuilder MapWebHooksApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api/webhook")
            .RequireAuthorization();

        api.MapPost($"/{{token}}", ReceiveDataAsync).AllowAnonymous();
        api.MapGet("/get/{id:int}", GetAsync);
        api.MapPost("/create", CreateAsync);
        api.MapPost("/updateTemplate", UpdateTemplateAsync);
        api.MapDelete("/delete/{id:int}", DeleteAsync);

        return api;
    }


    /// <summary>
    /// 接收WebHook请求
    /// </summary>
    /// <param name="token"></param>
    /// <param name="title"></param>
    /// <param name="data"></param>
    /// <param name="webHookApplicationService"></param>
    /// <returns></returns>
    private static async Task<IResult> ReceiveDataAsync(string token, dynamic data,
        IWebHookApplicationService webHookApplicationService)
    {
        string? title;
        string jsonData = Convert.ToString(data);
        using (var document = JsonDocument.Parse(jsonData))
        {
            var root = document.RootElement;
            // 获取 title 的值
            title = root.GetProperty("title").GetString();
        }

        var result = await webHookApplicationService.ReceiveDataAsync(token, title, data);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetAsync(int id, IWebHookApplicationService webHookApplicationService)
    {
        var webHook = await webHookApplicationService.GetAsync(id);
        return Results.Ok(webHook);
    }

    private static async Task<IResult> CreateAsync(CreateWebHookDto input,
        IWebHookApplicationService webHookApplicationService)
    {
        var result = await webHookApplicationService.CreateAsync(input);
        return Results.Ok(result);
    }

    private static async Task<IResult> UpdateTemplateAsync(int id, int newTemplateId,
        IWebHookApplicationService webHookApplicationService)
    {
        await webHookApplicationService.UpdateTemplateAsync(id, newTemplateId);
        return Results.Ok("更新成功");
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <param name="webHookApplicationService"></param>
    /// <returns></returns>
    private static async Task<IResult> DeleteAsync(int id, IWebHookApplicationService webHookApplicationService)
    {
        await webHookApplicationService.DeleteAsync(id);
        return Results.Ok("删除成功");
    }
}