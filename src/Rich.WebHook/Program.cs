using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rich.WebHook.Apis;
using Rich.WebHook.Application.Users;
using Rich.WebHook.Model.Users;
using Rich.WebHook.Model.WebHook;
using Template = Scriban.Template;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfig(builder.Configuration)
    .AddDbContextDependencyGroup(builder.Configuration)
    .AddDependencyGroup();

var configuration = builder.Configuration;

builder.Services.AddAuthorization();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rich.WebHook", Version = "v1" });

    // 配置 JWT 令牌输入
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.FromHours(5)
        };
    });

var app = builder.Build();
// DB迁移+初始化
HookConfigServiceCollectionExtensions.InitializeDatabase(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapWebHooksApi()
    .RequireAuthorization();


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

app.MapPost("User/Login", async (LoginInput input, IUserApplicationService userApplicationService) =>
{
    var token = await userApplicationService.Login(input.UserName, input.PassWord);
    return Results.Ok(token);
});


// 健康检查
app.MapGet("Health/Check", () => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));



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