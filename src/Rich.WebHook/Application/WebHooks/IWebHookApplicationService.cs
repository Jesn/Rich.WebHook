using Rich.WebHook.Application.WebHooks.Dto;

namespace Rich.WebHook.Application.WebHooks;

public interface IWebHookApplicationService : IRichApplicationService
{
    Task<string?> ReceiveDataAsync(string token, string? title, object data);

    Task CreateAsync(CreateWebHookDto input);

    Task<WebHookDetailDto?> GetAsync(int id);

    Task<WebHookDetailDto?> GetByTokenAsync(string token);

    Task DeleteAsync(int id);
}