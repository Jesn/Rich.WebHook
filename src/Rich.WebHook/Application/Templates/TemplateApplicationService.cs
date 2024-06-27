using System.Security.Authentication;
using Rich.WebHook.Application.Templates.Dto;
using Rich.WebHook.Application.Users;
using Rich.WebHook.EntityFramework.Model;
using Rich.WebHook.Repository.Templates;

namespace Rich.WebHook.Application.Templates;

public class TemplateApplicationService(
    ITemplateRepository templateRepository,
    IRichSession richSession)
    : ITemplateApplicationService
{
    public async Task<WebHookTemplate?> GetAsync(int id)
    {
        return await templateRepository.GetAsync(id);
    }

    public async Task<WebHookTemplate> AddAsync(CreateTemplateDto input)
    {
        var userId = richSession.UserId;
        if (userId is null) throw new AuthenticationException("用户未登录");

        // var fileName = $"{Guid.NewGuid():N}.tpl";
        // var directoryPath = Path.Combine(".", "Data", "WebHookTemplates");
        // var filePath = Path.Combine(directoryPath, fileName);
        // Directory.CreateDirectory(directoryPath);
        //
        // await File.WriteAllTextAsync(filePath, input.TemplateText);

        return await templateRepository.AddAsync(userId.Value, input.Name, input.TemplateText, input.Remark);
    }
}