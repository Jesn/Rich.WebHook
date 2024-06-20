namespace Rich.WebHook.EntityFramework.Model;

/// <summary>
/// WebHook 用户配置信息
/// </summary>
public class HookSetting:Entity
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// 模板路径
    /// </summary>
    public int TemplateId { get; set; }

    /// <summary>
    /// 来源站点
    /// </summary>
    public string Target { get; set; }

    /// <summary>
    /// 唯一Token
    /// </summary>

    public string Token { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    
    public DateTime CreateTime { get; init; }
}