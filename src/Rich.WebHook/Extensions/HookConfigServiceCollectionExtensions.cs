using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Rich.WebHook.Application;
using Rich.WebHook.Application.Users;
using Rich.WebHook.Dmain.Shared.Options;
using Rich.WebHook.EntityFramework;
using Rich.WebHook.EntityFramework.SeedData;
using Rich.WebHook.Repository;

namespace Microsoft.Extensions.DependencyInjection;

public static class HookConfigServiceCollectionExtensions
{
    public static IServiceCollection AddConfig(
        this IServiceCollection services, IConfiguration config)
    {
        services.Configure<JwtOptions>(config.GetSection("Jwt"));

        return services;
    }

    public static IServiceCollection AddDependencyGroup(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddHttpContextAccessor();
        services.AddTransient<IRichSession, RichSession>();

        #region Repository 依赖注入

        // 查找所有实现了 IRichRepository 接口的类型
        var repositoryTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } &&
                        t.GetInterfaces().Contains(typeof(IRichRepository)));

        foreach (var repositoryType in repositoryTypes)
        {
            var interfaceType = repositoryType.GetInterfaces()
                .FirstOrDefault(i => i != typeof(IRichRepository) && typeof(IRichRepository).IsAssignableFrom(i));

            if (interfaceType != null)
                services.AddScoped(interfaceType, repositoryType);
        }

        #endregion

        #region Application 应用层依赖注入

        var applicationServiceTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } &&
                        t.GetInterfaces().Contains(typeof(IRichApplicationService)));

        foreach (var applicationServiceType in applicationServiceTypes)
        {
            var interfaceType = applicationServiceType.GetInterfaces()
                .FirstOrDefault(i =>
                    i != typeof(IRichApplicationService) && typeof(IRichApplicationService).IsAssignableFrom(i));
            if (interfaceType != null)
                services.AddTransient(interfaceType, applicationServiceType);
        }

        #endregion

        return services;
    }

    /// <summary>
    /// 数据库配置
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddDbContextDependencyGroup(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySql(
                configuration.GetConnectionString("Default"),
                new MySqlServerVersion(new Version(8, 0, 21))
            );
        });

        return services;
    }

    /// <summary>
    /// 数据库迁移+种子数据初始化
    /// </summary>
    /// <param name="serviceProvider"></param>
    public static void InitializeDatabase(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        DatabaseInitializer.Initialize(context);
    }
}