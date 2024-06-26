namespace Rich.WebHook.Application.Templates.Dto;

public record CreateTemplateDto(
    string Name,
    string TemplateText,
    string Remark);