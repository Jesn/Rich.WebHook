using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.EntityFramework;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<UserInfo> Users { get; init; }
    public DbSet<WebHookSetting> WebHookSettings { get; init; }

    public DbSet<WebHookReceiver> WebHookReceivers { get; init; }

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

        #region WebHokSetting

        modelBuilder.Entity<WebHookSetting>()
            .Property(x => x.Title)
            .HasMaxLength(50)
            .IsRequired();
        modelBuilder.Entity<WebHookSetting>()
            .Property(x => x.Remark)
            .HasMaxLength(200);

        modelBuilder.Entity<WebHookSetting>()
            .Property(x => x.Token)
            .HasMaxLength(60);

        modelBuilder.Entity<WebHookSetting>()
            .HasIndex(x => x.UserId);

        modelBuilder.Entity<WebHookSetting>()
            .HasIndex(x => x.Token)
            .IsUnique();

        #endregion

        #region WebHookReceiver

        modelBuilder.Entity<WebHookReceiver>()
            .HasOne(wr => wr.WebHook)
            .WithMany(wr => wr.Receivers)
            .HasForeignKey(wr => wr.WebHookId);

        #endregion
    }
}