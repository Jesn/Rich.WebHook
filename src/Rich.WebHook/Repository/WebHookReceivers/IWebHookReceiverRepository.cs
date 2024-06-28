using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.Repository.WebHookReceivers;

public interface IWebHookReceiverRepository : IRichRepository
{
    Task<IEnumerable<WebHookReceiver>> GetReceiversByWebHookIds(List<int> webhookIds);
    Task<IEnumerable<WebHookReceiver>> GetReceiversByWebHookId(int webhookId);
    Task BatchAsync(List<WebHookReceiver> list);

    Task DeleteAsync(int webhookId);

    
}