namespace Rich.WebHook.Model.WebHook;

public  class CreateWebhookInput
{
    /// <summary>
    /// Hook来自那个系统
    /// </summary>
    public required string Source { get; set; }

    /// <summary>
    /// 模板内容
    /// </summary>
    public required string TemplateText { get; set; }
    
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
    
}