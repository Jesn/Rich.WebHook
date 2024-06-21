using Microsoft.EntityFrameworkCore;
using Rich.WebHook.Common;
using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.EntityFramework.SeedData;

public class DatabaseInitializer
{
    public static void Initialize(ApplicationDbContext dbContext)
    {
        dbContext.Database.Migrate();

        // 检查是否已经存在数据
        if (dbContext.Users.Any())
        {
            return;
        }

        var passWord = "123456";
        var passSaltHash = PasswordHasher.HashPasswordWithSalt(passWord);

        dbContext.Users.AddRangeAsync(new UserInfo()
        {
            Id = 1,
            UserName = "admin",
            PassWordSalt = passSaltHash.Item1,
            PassWordHash = passSaltHash.Item2,
            Email = "admin@rich.cn"
        });

        dbContext.SaveChangesAsync();
    }
}