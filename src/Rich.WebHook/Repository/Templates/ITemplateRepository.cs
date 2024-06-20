using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.Repository.Templates;

public interface ITemplateRepository : IRichRepository
{
    Task<Template?> GetAsync(int id);

    Task AddAsync(Template template);
}