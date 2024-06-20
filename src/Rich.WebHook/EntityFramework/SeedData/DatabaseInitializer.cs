using Microsoft.EntityFrameworkCore;
using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.EntityFramework.SeedData;

public class DatabaseInitializer
{
    public static void Initialize(ApplicationDbContext dbContext)
    {
        dbContext.Database.MigrateAsync();

        // 检查是否已经存在数据
        if (dbContext.Users.Any())
        {
            return;
        }

        dbContext.Users.AddRangeAsync(new User() { Id = 1, Name = "admin", Email = "admin@rich.cn" });

        dbContext.SaveChangesAsync();
    }
}