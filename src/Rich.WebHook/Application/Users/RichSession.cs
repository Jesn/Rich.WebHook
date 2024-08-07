using System.Net;
using Microsoft.Extensions.DependencyInjection.Exceptions;
using Rich.WebHook.Dmain.Shared.Const;

namespace Rich.WebHook.Application.Users;

public class RichSession(IHttpContextAccessor httpContextAccessor)
    : IRichSession
{
    public int? UserId
    {
        get
        {
            var userIdClaim =
                httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c =>
                    c.Type == ClaimRichConst.UserId);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return null;
            }

            return userId;
        }
    }

    public string? UserName
    {
        get
        {
            var userName = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimRichConst.UserName)?.Value;
            return userName;
        }
    }

    public string? Email
    {
        get
        {
            var email = httpContextAccessor.HttpContext?.User?.FindFirst(x => x.Type == ClaimRichConst.Email)?.Value;
            return email;
        }
    }

    public int GetUserId()
    {
        if (UserId == null) throw new UserFriendException(HttpStatusCode.Unauthorized, "Token已过期，请重新登录！");
        return UserId.Value;
    }
}