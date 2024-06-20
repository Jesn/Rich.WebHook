using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.Repository.UserTemplates;

public interface IUserTemplateRepository : IRichRepository
{
    Task AddAsync(UserTemplate userTemplate);
}