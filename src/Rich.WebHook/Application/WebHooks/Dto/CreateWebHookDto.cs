namespace Rich.WebHook.Application.WebHooks.Dto;

public record CreateWebHookDto(
    int TemplateId,
    string Remark
);