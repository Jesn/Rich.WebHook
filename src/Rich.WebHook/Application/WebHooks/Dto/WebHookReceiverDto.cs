using Rich.WebHook.Dmain.Shared.MessageForward;

namespace Rich.WebHook.Application.WebHooks.Dto;

public class WebHookReceiverDto
{
    public ReceiveClientEnum Client { get; set; }
    public MemberTypeEnum MemberType { get; set; }
    public List<string> Receivers { get; set; } = new();
}