FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src/expenso_app
COPY . .
RUN dotnet restore src/Api/Expenso.Api/Expenso.Api.csproj

FROM build AS publish
RUN dotnet publish src/Api/Expenso.Api/Expenso.Api.csproj -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER app
ENTRYPOINT ["dotnet", "Expenso.Api.dll"]