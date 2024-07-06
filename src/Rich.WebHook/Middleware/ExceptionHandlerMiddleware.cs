using Microsoft.Extensions.DependencyInjection.Exceptions;
using Newtonsoft.Json;
using Rich.WebHook.ViewModel;

namespace Rich.WebHook.Middleware;

/// <summary>
/// 异常类中间件
/// </summary>
/// <param name="next"></param>
public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (UserFriendException ex)
        {
            await HandleUserFriendExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// 自定义异常类处理
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    private static Task HandleUserFriendExceptionAsync(HttpContext context, UserFriendException exception)
    {
        context.Response.StatusCode = exception.StatusCode;
        context.Response.ContentType = "application/json";

        var response = new ApiResponse<object?>(
            exception.StatusCode,
            exception.Message,
            null
        );

        Console.WriteLine(JsonConvert.SerializeObject(response));

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }

    /// <summary>
    /// 系统异常类处理
    /// </summary>
    /// <param name="context"></param>
    /// <param name="innerException"></param>
    /// <returns></returns>
    private static Task HandleExceptionAsync(HttpContext context, Exception innerException)
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var response = new ApiResponse<object?>(
            500,
            "An unexpected error occurred",
            null
        );

        Console.WriteLine(JsonConvert.SerializeObject(innerException.ToString()));

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}