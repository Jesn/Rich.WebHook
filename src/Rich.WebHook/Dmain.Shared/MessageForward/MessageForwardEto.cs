namespace Rich.WebHook.Dmain.Shared.MessageForward;

public class MessageForwardEto
{
    /// <summary>
    /// 接收的客户端
    /// </summary>
    public ReceiveClientEnum Client { get; set; }

    /// <summary>
    /// 接收者
    /// </summary>
    public List<string> Receiver { get; set; } = new();

    /// <summary>
    /// 接收者类型
    /// </summary>
    public MemberTypeEnum Type { get; set; }
}