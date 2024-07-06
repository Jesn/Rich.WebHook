using Newtonsoft.Json;
using Rich.WebHook.ViewModel;

namespace Rich.WebHook.Middleware;

public class ResponseMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await next(context);
        }
        catch (Exception)
        {
            // 在上面的修改中，如果在调用_next(context)时发生异常，捕获异常并重置context.Response.Body为originalBodyStream，然后重新抛出异常。
            // 这样，ExceptionHandlerMiddleware可以正常处理响应流并写入错误响应。
            context.Response.Body = originalBodyStream;
            throw;
        }

        context.Response.Body = originalBodyStream;
        responseBody.Seek(0, SeekOrigin.Begin);
        var bodyText = await new StreamReader(responseBody).ReadToEndAsync();
        responseBody.Seek(0, SeekOrigin.Begin);

        object? bodyObject;
        try
        {
            bodyObject = JsonConvert.DeserializeObject(bodyText);
        }
        catch
        {
            bodyObject = bodyText;
        }

        var apiResponse = new ApiResponse<object?>(
            context.Response.StatusCode,
            context.Response.StatusCode == 200 ? "Success" : "Error",
            bodyObject
        );

        // 清除当前响应
        context.Response.Clear();
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonConvert.SerializeObject(apiResponse));
    }
}