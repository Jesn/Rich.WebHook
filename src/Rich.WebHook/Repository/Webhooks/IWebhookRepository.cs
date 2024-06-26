using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.Repository.Webhooks;

public interface IWebhookRepository : IRichRepository
{
    Task<WebHookSetting?> GetAsync(int id);

    Task<WebHookSetting?> GetByTokenAsync(string token);

    Task<WebHookSetting?> AddAsync(WebHookSetting model);

    Task UpdateTemplate(int id, int templateId);

    Task Delete(int id);
}