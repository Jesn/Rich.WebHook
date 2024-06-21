using System.Xml.Linq;

namespace Rich.WebHook.Application.Templates;

public interface ITemplateApplicationService : IRichApplicationService
{
    void AddTemplateAsync();
}