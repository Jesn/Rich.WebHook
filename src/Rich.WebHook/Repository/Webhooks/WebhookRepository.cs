using System.Data;
using Microsoft.EntityFrameworkCore;
using Rich.WebHook.EntityFramework;
using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.Repository.Webhooks;

public class WebhookRepository(ApplicationDbContext context) : IWebhookRepository
{
    private IWebhookRepository _webhookRepositoryImplementation;

    public async Task<WebHookSetting?> GetAsync(int userId, int id)
    {
        return await context.WebHookSettings.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
    }

    public async Task<WebHookSetting?> GetByTokenAsync(string token)
    {
        var webHook = await context.WebHookSettings.FirstOrDefaultAsync(x => x.Token == token);
        return webHook;
    }

    /// <summary>
    /// 新增数据
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<WebHookSetting?> AddAsync(WebHookSetting model)
    {
        var webHook = (await context.WebHookSettings.AddAsync(model)).Entity;
        await context.SaveChangesAsync();
        return webHook;
    }


    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DataException"></exception>
    public async Task Delete(int id)
    {
        var webHook = await context.WebHookSettings.FindAsync(id);
        if (webHook is null)
            throw new DataException("未找到该条数据");

        context.WebHookSettings.Remove(webHook);
        await context.SaveChangesAsync();
    }
}