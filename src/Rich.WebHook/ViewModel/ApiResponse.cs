namespace Rich.WebHook.ViewModel;

public class ApiResponse<T>(int statusCode, string message, T data)
{
    public int StatusCode { get; set; } = statusCode;
    public string Message { get; set; } = message;
    public T Data { get; set; } = data;
}