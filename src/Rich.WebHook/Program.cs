using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rich.WebHook.Apis;
using Rich.WebHook.Middleware;

var builder = WebApplication.CreateBuilder(args);

if (builder.Configuration.GetSection("AgileConfig").Exists())
    builder.Host.UseAgileConfig();

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
});

// CORS 跨域
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowSpecificOrigin",
//         builder => builder.WithOrigins("http://example.com")
//             .AllowAnyHeader()
//             .AllowAnyMethod());
// });

builder.Services
    .AddAgileConfig1(builder.Configuration)
    .AddOptionConfig(builder.Configuration)
    .AddDbContextDependencyGroup(builder.Configuration)
    .AddDependencyGroup()
    .AddAuthorization()
    .AddLogging(loggingBuilder =>
    {
        loggingBuilder.AddConsole();
        loggingBuilder.AddDebug();
    });


builder.Services.AddTransient<WebHookApi>();
var configuration = builder.Configuration;

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
            Array.Empty<string>()
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
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? string.Empty)),
            ClockSkew = TimeSpan.FromHours(5)
        };
    });


var app = builder.Build();

// CORS 跨域
//app.UseCors("AllowSpecificOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<ResponseMiddleware>();



app.UseHttpsRedirection();

// 健康检查
app.MapGet("Health/Check", () => "ok");

app.MapAccountApi();

var webHookApi = app.Services.GetRequiredService<WebHookApi>();
webHookApi.MapWebHooksApi(app);

app.UseAuthentication();
app.UseAuthorization();

// DB迁移+初始化
HookConfigServiceCollectionExtensions.InitializeDatabase(app.Services);

app.Run();