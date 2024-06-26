# 使用更小的基础镜像
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 80 443

# 使用sdk镜像进行构建
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Rich.WebHook/Rich.WebHook.csproj", "src/Rich.WebHook/"]
RUN dotnet restore "src/Rich.WebHook/Rich.WebHook.csproj"
COPY . .
WORKDIR "/src/src/Rich.WebHook"
RUN dotnet build "Rich.WebHook.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "Rich.WebHook.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 最终镜像
FROM base AS final

WORKDIR /app
COPY --from=build /app/publish .

# 使用非root用户运行应用
RUN adduser -u 5678 -D appuser
# 设置 localhost.pfx 文件权限
RUN chown appuser:appuser /app/localhost.pfx && chmod 600 /app/localhost.pfx
# 切换到 appuser
USER appuser

# 设置环境变量
ENV ASPNETCORE_URLS="https://+:443;http://+:80"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/localhost.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=password

ENTRYPOINT ["dotnet", "Rich.WebHook.dll"]