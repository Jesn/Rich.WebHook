using Microsoft.EntityFrameworkCore;
using Rich.WebHook.EntityFramework;
using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.Repository.WebHookReceivers;

public class WebHookReceiverRepository(ApplicationDbContext context) : IWebHookReceiverRepository
{
    public async Task<IEnumerable<WebHookReceiver>> GetReceiversByWebHookIds(List<int> webhookIds)
    {
        var list = await context.WebHookReceivers
            .Where(x => webhookIds.Contains(x.WebHookId))
            .ToListAsync();
        return list;
    }

    public async Task<IEnumerable<WebHookReceiver>> GetReceiversByWebHookId(int webhookId)
    {
        var list = await context.WebHookReceivers
            .Where(x => x.WebHookId == webhookId)
            .ToListAsync();
        return list;
    }

    public async Task BatchAsync(List<WebHookReceiver> list)
    {
        await context.WebHookReceivers.AddRangeAsync(list);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int webhookId)
    {
        var receivers = await context.WebHookReceivers
            .Where(x => x.WebHookId == webhookId)
            .ToListAsync();
        context.WebHookReceivers.RemoveRange(receivers);
        await context.SaveChangesAsync();
    }
}