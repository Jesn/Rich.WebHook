using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.Repository.Templates;

public interface ITemplateRepository : IRichRepository
{
    Task<WebHookTemplate?> GetAsync(int id);

    Task<WebHookTemplate> AddAsync(int userId, string name, string fileName, string remark);
}