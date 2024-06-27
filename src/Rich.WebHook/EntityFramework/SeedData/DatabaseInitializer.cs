using System.Text;
using Microsoft.EntityFrameworkCore;
using Rich.WebHook.Common;
using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.EntityFramework.SeedData;

public class DatabaseInitializer
{
    public static void Initialize(ApplicationDbContext dbContext, ILogger<DatabaseInitializer> logger)
    {
        dbContext.Database.Migrate();

        // 检查是否已经存在数据
        if (dbContext.Users.Any())
        {
            return;
        }

        var passWord = PasswordHasher.GeneratePassword(15);
        Console.WriteLine($"当前 admin 账号随机密码:{passWord}");
        logger.LogInformation($"当前 admin 账号随机密码:{passWord}", ConsoleColor.Red);

        var passSaltHash = PasswordHasher.HashPasswordWithSalt(passWord);
        var passWordSecret = PasswordHasher.ToBase64(passSaltHash.Item1, passSaltHash.Item2);

        dbContext.Users.AddRangeAsync(new UserInfo()
        {
            Id = 1,
            UserName = "admin",
            PassWord = passWordSecret,
            Email = "admin@rich.cn"
        });

        dbContext.SaveChangesAsync();
    }
}