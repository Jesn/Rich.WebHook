using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.Repository.Webhooks;

public interface IWebhookRepository : IRichRepository
{
    Task<WebHookSetting?> GetAsync(int userId,int id);

    Task<WebHookSetting?> GetByTokenAsync(string token);

    Task<WebHookSetting?> AddAsync(WebHookSetting model);

    Task Delete(int id);
}