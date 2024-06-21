using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.Repository.Users;

public interface IUserRepository : IRichRepository
{
    ValueTask<UserInfo?> GetUserByIdAsync(int id);
    
    ValueTask<UserInfo?> GetUserByNameAsync(string userName);

}