using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.Application.Users;

public interface IUserApplicationService: IRichApplicationService
{
    ValueTask<User?> GetUserByIdAsync(int id);
}