namespace Rich.WebHook.Dmain.Shared.MessageForward;

public class TextMessageEto
{
    public TextMessageEto(string title, string content)
    {
        Title = title;
        Content = content;
    }

    public string Title { get; set; }
    public string Content { get; set; }

    public List<MessageForwardEto> MessageForward { get; set; } = new();
}