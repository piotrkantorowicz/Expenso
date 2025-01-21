FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src/expenso_app
COPY ["./src/Api/Expenso.Api/Expenso.Api.csproj", "expenso_app/"]
RUN dotnet restore "./src/Api/Expenso.Api/Expenso.Api.csproj"
COPY . .
RUN dotnet publish "./src/Api/Expenso.Api/Expenso.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./aspnetapp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Expenso.Api.dll"]