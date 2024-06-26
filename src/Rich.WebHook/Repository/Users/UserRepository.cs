using System.Data;
using Microsoft.EntityFrameworkCore;
using Rich.WebHook.EntityFramework;
using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.Repository.Users;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<UserInfo> AddAsync(string userName, string passWord, string email)
    {
        var user = new UserInfo()
        {
            UserName = userName,
            PassWord = passWord,
            Email = email
        };
        user = (await context.Users.AddAsync(user)).Entity;
        await context.SaveChangesAsync();
        return user;
    }

    public ValueTask<UserInfo?> GetUserByIdAsync(int id)
    {
        return context.Users.FindAsync(id);
    }

    public async ValueTask<UserInfo?> GetUserByNameAsync(string userName)
    {
        return await context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
    }
}