namespace Rich.WebHook.Application.Users;

public interface IRichSession
{
    int? UserId { get;}
    string? UserName { get; }
    string? Email { get;  }
}