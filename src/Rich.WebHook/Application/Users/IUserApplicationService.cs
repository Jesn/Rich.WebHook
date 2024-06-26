using Rich.WebHook.Application.Users.Dto;
using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.Application.Users;

public interface IUserApplicationService : IRichApplicationService
{
    ValueTask<UserDto> RegisterAsync(string userName, string passWord, string email);
    ValueTask<string> Login(string userName, string passWord);
    ValueTask<UserInfo?> GetUserByIdAsync(int id);
}