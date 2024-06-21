using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rich.WebHook.Application.Users;
using Rich.WebHook.Model.WebHook;

namespace Rich.WebHook.Apis;

public static class WebHookApi
{
    public static RouteGroupBuilder MapWebHooksApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api/webhooks");

        // 创建WebHook模板
        api.MapPost("/create",
            async (CreateWebhookInput input, IUserApplicationService userApplicationService, 
                IRichSession richSession) =>
            {
                // var fileName = $"{Guid.NewGuid():N}.tpl";
                // if (!string.IsNullOrEmpty(input.TemplateText))
                //     await File.WriteAllTextAsync($"./Data/Templates/{fileName}",input.TemplateText);
                //

                var userid = richSession.UserId;
                var username = richSession.UserName;
                var email = richSession.Email;

                var user = await userApplicationService.GetUserByIdAsync(1);
                return Results.Ok(new { Message = "Webhook created successfully", User = user });
            });


        api.MapPost("/{token}", (string token, dynamic data) => { return token; })
            .AllowAnonymous();

        return api;
    }
}