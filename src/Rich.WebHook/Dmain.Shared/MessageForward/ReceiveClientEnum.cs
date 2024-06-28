namespace Rich.WebHook.Dmain.Shared.MessageForward;

/// <summary>
/// 接收客户端
/// </summary>
public enum ReceiveClientEnum
{
    WeChat = 0,
    Telegram = 1,
    Mq = 2,
    Http = 3
}