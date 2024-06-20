using Microsoft.AspNetCore.Mvc;
using Rich.WebHook.Application.Users;
using Rich.WebHook.Model.WebHook;
using Scriban;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfig(builder.Configuration)
    .AddDbContextDependencyGroup(builder.Configuration)
    .AddDependencyGroup();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// DB迁移+初始化
HookConfigServiceCollectionExtensions.InitializeDatabase(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

// 健康检查
app.MapGet("Health/Check", () => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

// 创建WebHook模板
app.MapPost("webhook/create", async (CreateWebhookInput input, IUserApplicationService userApplicationService) =>
{
    // var fileName = $"{Guid.NewGuid():N}.tpl";
    // if (!string.IsNullOrEmpty(input.TemplateText))
    //     await File.WriteAllTextAsync($"./Data/Templates/{fileName}",input.TemplateText);
    //
    
    
    var user = await userApplicationService.GetUserByIdAsync(1);
    return Results.Ok(new { Message = "Webhook created successfully", User = user });
});

app.MapPost("webhook", ([FromQuery] string system, [FromBody] dynamic data) =>
{
    // 定义模板
    var template = Template.Parse(@"Hello {{ name }}! You are {{ age }} years . ");

    // 渲染模板，传入myObject作为数据上下文
    var result = template.Render(data);
    return Task.FromResult(result);
});




app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}