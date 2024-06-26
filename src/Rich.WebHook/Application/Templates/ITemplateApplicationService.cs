using System.Xml.Linq;
using Rich.WebHook.Application.Templates.Dto;
using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.Application.Templates;

public interface ITemplateApplicationService : IRichApplicationService
{
    Task<WebHookTemplate> AddAsync(CreateTemplateDto input);
}