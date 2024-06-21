using Microsoft.EntityFrameworkCore;
using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.EntityFramework;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<UserInfo> Users { get; set; }
    public DbSet<Template> Templates { get; set; }
    public DbSet<UserTemplate> UserTemplates { get; set; }
    public DbSet<HookSetting> HookSettings { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // 配置所有继承自 Entity 的实体的主键为自增
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType.IsSubclassOf(typeof(Entity)))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property("Id")
                    .ValueGeneratedOnAdd();
            }
        }
    }
}