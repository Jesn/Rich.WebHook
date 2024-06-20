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
    public async Task<Template?> GetAsync(int id)
    {
        return await context.Templates.FindAsync(id);
    }

    /// <summary>
    /// 新增模板
    /// </summary>
    /// <param name="template"></param>
    public async Task AddAsync(Template template)
    {
        await context.Templates.AddAsync(template);
    }
}