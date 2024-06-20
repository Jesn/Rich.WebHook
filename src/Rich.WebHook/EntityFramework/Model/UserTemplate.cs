namespace Rich.WebHook.EntityFramework.Model;


public class UserTemplate:Entity
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// 模板GUID
    /// </summary>
    public int TemplateGuid { get; set; }
}