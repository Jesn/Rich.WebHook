using Rich.WebHook.Dmain.Shared.MessageForward;

namespace Rich.WebHook.Application.WebHooks.Dto;

public class CreateWebHookDto
{
    public string Title { get; set; }
    public string TemplateText { get; set; }
    public string Remark { get; set; }

    public List<WebHookReceiverDto> Receivers { get; set; } = new();
}