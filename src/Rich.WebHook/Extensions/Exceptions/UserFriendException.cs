using System.Net;

namespace Microsoft.Extensions.DependencyInjection.Exceptions;

public class UserFriendException : Exception
{
    public int StatusCode { get; }

    public UserFriendException()
    {
    }

    public UserFriendException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }

    public UserFriendException(int statusCode, string message, Exception innerException)
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }

    public UserFriendException(HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode.GetHashCode();
    }

    public UserFriendException(HttpStatusCode statusCode, string message, Exception innerException)
        : base(message, innerException)
    {
        StatusCode = statusCode.GetHashCode();
    }
}