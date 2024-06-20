using Rich.WebHook.EntityFramework;
using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.Repository.UserTemplates;

public class UserTemplateRepository(ApplicationDbContext context): IUserTemplateRepository
{
    public async Task AddAsync(UserTemplate userTemplate)
    {
        await context.UserTemplates.AddAsync(userTemplate);
    }
}