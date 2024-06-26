namespace Rich.WebHook.EntityFramework.Model;

/// <summary>
/// WebHook 用户配置信息
/// </summary>
public class WebHookSetting : Entity
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 唯一Token
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 模板内容
    /// </summary>
    public string TemplateText { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>

    public DateTime CreateTime { get; set; }
    
    public virtual ICollection<WebHookReceiver> Receivers { get; set; }
}