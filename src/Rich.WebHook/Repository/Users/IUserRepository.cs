using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.Repository.Users;

public interface IUserRepository : IRichRepository
{
    ValueTask<User?> GetUserByIdAsync(int id);
}