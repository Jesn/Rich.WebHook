using Rich.WebHook.Dmain.Shared.MessageForward;

namespace Rich.WebHook.EntityFramework.Model;

public class WebHookReceiver : Entity
{
    public WebHookReceiver(int webHookId, ReceiveClientEnum client, List<string> receivers, MemberTypeEnum memberType)
    {
        WebHookId = webHookId;
        Client = client;
        Receivers = receivers;
        MemberType = memberType;
    }

    public int WebHookId { get; set; }
    public ReceiveClientEnum Client { get; set; }
    public MemberTypeEnum MemberType { get; set; }

    public List<string> Receivers { get; set; }

    public virtual WebHookSetting WebHook { get; set; }
}