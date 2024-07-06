using System.ComponentModel.DataAnnotations;
using Rich.WebHook.Dmain.Shared.MessageForward;

namespace Rich.WebHook.Application.WebHooks.Dto;

public class CreateWebHookDto
{
    [Required(ErrorMessage = "标题不能为空")] public string Title { get; set; }
    public string TemplateText { get; set; }

    [MaxLength(ErrorMessage = "最多只能输入200个字符")]
    public string Remark { get; set; }

    [Required(ErrorMessage = "接收方不能为空")] public List<WebHookReceiverDto> Receivers { get; set; } = new();
}