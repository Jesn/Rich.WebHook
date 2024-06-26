using Rich.WebHook.Application.WebHooks.Dto;
using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.Application.WebHooks;

public interface IWebHookApplicationService : IRichApplicationService
{
    Task<string?> ReceiveDataAsync(string token, string? title, object data);

    Task<WebHookSetting?> CreateAsync(CreateWebHookDto input);

    Task<WebHookDetailDto?> GetAsync(int id);

    Task<WebHookDetailDto?> GetByTokenAsync(string token);

    Task UpdateTemplateAsync(int id, int templateGuid);

    Task DeleteAsync(int id);
}