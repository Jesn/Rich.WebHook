using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.Application.Users;

public interface IUserApplicationService : IRichApplicationService
{
    ValueTask<string> Login(string userName, string passWord);
    ValueTask<UserInfo?> GetUserByIdAsync(int id);
}