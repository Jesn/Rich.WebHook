namespace Rich.WebHook.Application.Users;

public interface IRichSession
{
    long? UserId { get;}
    string? UserName { get; }
    string? Email { get;  }
}