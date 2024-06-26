using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.Repository.Users;

public interface IUserRepository : IRichRepository
{
    Task<UserInfo> AddAsync(string userName, string passWord, string email);
    ValueTask<UserInfo?> GetUserByIdAsync(int id);

    ValueTask<UserInfo?> GetUserByNameAsync(string userName);
}