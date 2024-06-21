using Microsoft.EntityFrameworkCore;
using Rich.WebHook.EntityFramework;
using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.Repository.Users;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public ValueTask<UserInfo?> GetUserByIdAsync(int id)
    {
        return context.Users.FindAsync(id);
    }

    public async ValueTask<UserInfo?> GetUserByNameAsync(string userName)
    {
        return await context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
    }

 
}