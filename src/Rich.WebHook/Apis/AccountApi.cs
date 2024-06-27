using Microsoft.Extensions.Options;
using Rich.WebHook.Application.Users;
using Rich.WebHook.Dmain.Shared.Options;
using Rich.WebHook.Model.Users;

namespace Rich.WebHook.Apis;

public static class AccountApi
{
    public static RouteGroupBuilder MapAccountApi(this IEndpointRouteBuilder app)
    {
        var systemConfigOptions = app.ServiceProvider.GetRequiredService<IOptions<SystemConfigOptions>>();

        var api = app.MapGroup("/api/account")
            .RequireAuthorization();

        // 是否开启注册
        if (systemConfigOptions.Value.IsRegister)
            api.MapPost("/register", RegisterAsync).AllowAnonymous();

        api.MapPost("/login", LoginAsync).AllowAnonymous();

        return api;
    }

    /// <summary>
    /// 用户注册
    /// </summary>
    /// <param name="input"></param>
    /// <param name="userApplicationService"></param>
    /// <returns></returns>
    private static async Task<IResult> RegisterAsync(RegisterInput input,
        IUserApplicationService userApplicationService)
    {
        if (input.PassWord != input.PassWordTwo)
            return TypedResults.BadRequest("两次密码不一致");

        var user = await userApplicationService.RegisterAsync(input.UserName, input.PassWord, input.Email);

        return TypedResults.Ok(user);
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="input"></param>
    /// <param name="userApplicationService"></param>
    /// <returns></returns>
    private static async Task<IResult> LoginAsync(LoginInput input, IUserApplicationService userApplicationService)
    {
        if (string.IsNullOrEmpty(input.UserName) || string.IsNullOrEmpty(input.PassWord))
        {
            return TypedResults.BadRequest("用户名或密码不能为空！");
        }

        var token = await userApplicationService.Login(input.UserName, input.PassWord);
        return Results.Ok(token);
    }
}