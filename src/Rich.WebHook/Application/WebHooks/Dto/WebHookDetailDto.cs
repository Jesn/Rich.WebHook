using Rich.WebHook.Dmain.Shared.MessageForward;

namespace Rich.WebHook.Application.WebHooks.Dto;

public class WebHookDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string TemplateText { get; set; }
    public string Remark { get; set; }
    public string Url { get; set; }

    public List<WebHookReceiverDto>? Receivers { get; set; } 
}