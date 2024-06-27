namespace Rich.WebHook.EntityFramework.Model;

public class WebHookTemplate : Entity
{
    /// <summary>
    /// 模板名称
    /// </summary>
    public string Name { get; set; }

    public string Content { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public int CreaterId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateAt { get; set; }
}