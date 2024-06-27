using Microsoft.EntityFrameworkCore;
using Rich.WebHook.EntityFramework;
using Rich.WebHook.EntityFramework.Model;

namespace Rich.WebHook.Repository.Templates;

public class TemplateRepository(ApplicationDbContext context) : ITemplateRepository
{
    /// <summary>
    /// 按照ID获取模板信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<WebHookTemplate?> GetAsync(int id)
    {
        return await context.WebHookTemplates.FindAsync(id);
    }

    /// <summary>
    /// 新增模板
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="name"></param>
    /// <param name="content"></param>
    /// <param name="remark"></param>
    public async Task<WebHookTemplate> AddAsync(int userId, string name, string content, string remark)
    {
        var template = new WebHookTemplate()
        {
            Name = name,
            Content = content,
            Remark = remark,
            CreaterId = userId,
            CreateAt = DateTime.Now
        };

        template = (await context.WebHookTemplates.AddAsync(template)).Entity;
        await context.SaveChangesAsync();
        return template;
    }
}