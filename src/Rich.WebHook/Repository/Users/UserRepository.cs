using Rich.WebHook.EntityFramework;
using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.Repository.Users;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public ValueTask<User?> GetUserByIdAsync(int id)
    {
        return context.Users.FindAsync(id);
    }
}