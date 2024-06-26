using Rich.WebHook.Application.Templates;
using Rich.WebHook.Application.Templates.Dto;

namespace Rich.WebHook.Apis;

/// <summary>
/// 模板/// </summary>
public static class TemplateApi
{
    public static RouteGroupBuilder MapTemplateApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api/template")
            .RequireAuthorization();

        api.MapGet("/{id:int}", GetAsync);
        api.MapPost("/create", CreateAsync);


        return api;
    }

    private static async Task<IResult> GetAsync(int id)
    {
        return Results.Ok("");
    }

    private static async Task<IResult> CreateAsync(CreateTemplateDto input,ITemplateApplicationService templateApplicationService)
    {
        var template = await templateApplicationService.AddAsync(input);
        return Results.Ok(template);
    }
}