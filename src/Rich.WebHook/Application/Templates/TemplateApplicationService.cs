using Rich.WebHook.EntityFramework.Model;
using Rich.WebHook.Repository.Templates;
using Rich.WebHook.Repository.UserTemplates;

namespace Rich.WebHook.Application.Templates;

public class TemplateApplicationService(
    ITemplateRepository templateRepository,
    IUserTemplateRepository userTemplateRepository)
    : ITemplateApplicationService
{
    public void AddTemplateAsync()
    {
        
        throw new NotImplementedException();
    }
}