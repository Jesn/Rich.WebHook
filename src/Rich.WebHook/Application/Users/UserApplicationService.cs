using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Exceptions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rich.WebHook.Application.Users.Dto;
using Rich.WebHook.Common;
using Rich.WebHook.Dmain.Shared.Const;
using Rich.WebHook.Dmain.Shared.Options;
using Rich.WebHook.EntityFramework.Model;
using Rich.WebHook.Repository.Users;

namespace Rich.WebHook.Application.Users;

public class UserApplicationService(IUserRepository userRepository, IOptions<JwtOptions> jwtOptions)
    : IUserApplicationService
{
    private JwtOptions JwtOption { get; } = jwtOptions.Value;

    /// <summary>
    /// 注册用户
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="passWord"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    /// <exception cref="DataException"></exception>
    public async ValueTask<UserDto> RegisterAsync(string userName, string passWord, string email)
    {
        var user = await userRepository.GetUserByNameAsync(userName);

        if (user is not null) throw new DataException("当前用户名已存在");

        var passWordSaltHash = PasswordHasher.HashPasswordWithSalt(passWord);
        var passWordSecret = PasswordHasher.ToBase64(passWordSaltHash.Item1, passWordSaltHash.Item2);
        user = await userRepository.AddAsync(userName, passWordSecret, email);

        return new UserDto() { UserId = user.Id, UserName = user.UserName, Email = user.Email };
    }


    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="passWord"></param>
    /// <returns></returns>
    /// <exception cref="DataException"></exception>
    public async ValueTask<string> Login(string userName, string passWord)
    {
        var user = await userRepository.GetUserByNameAsync(userName);
        if (user is null)
            throw new UserFriendException(HttpStatusCode.Unauthorized, "未找到该用户");

        var passWordIsOk = PasswordHasher.VerifyPasswordWithSalt(passWord, user.PassWord);
        if (!passWordIsOk)
            throw new UserFriendException(HttpStatusCode.Unauthorized, "密码不正确");

        var token = GenerateJwtToken(user, JwtOption);

        return token;
    }

    /// <summary>
    /// 生成Jwt Token
    /// </summary>
    /// <param name="userInfo"></param>
    /// <param name="jwtOption"></param>
    /// <returns></returns>
    private string GenerateJwtToken(UserInfo userInfo, JwtOptions jwtOption)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimRichConst.UserId, userInfo.Id.ToString()),
            new Claim(ClaimRichConst.UserName, userInfo.UserName),
            new Claim(ClaimRichConst.Email, userInfo.Email),
        };

        var token = new JwtSecurityToken(
            issuer: jwtOption.Issuer,
            audience: jwtOption.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);

        // var identity = new ClaimsIdentity(claims, "login");
        // var principal = new ClaimsPrincipal(identity);
        //
        // // 设置当前线程的 Principal
        // Thread.CurrentPrincipal = principal;
        //
        // // 如果使用的是 ASP.NET Core，还需要设置 HttpContext.User
        // if (HttpContext.user != null)
        // {
        //     HttpContext.Current.User = principal;
        // }

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ValueTask<UserInfo?> GetUserByIdAsync(int id)
    {
        return userRepository.GetUserByIdAsync(id);
    }
}