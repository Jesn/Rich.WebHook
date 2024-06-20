using Rich.WebHook.EntityFramework.Model;
using Rich.WebHook.Repository.Users;

namespace Rich.WebHook.Application.Users;

public class UserApplicationService(IUserRepository userRepository) : IUserApplicationService
{
    public ValueTask<User?> GetUserByIdAsync(int id)
    {
        return userRepository.GetUserByIdAsync(id);
    }
}