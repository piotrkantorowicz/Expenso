FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine as builder
WORKDIR /app
COPY . .
RUN dotnet restore Src/Api/Expenso.Api/Expenso.Api.csproj && dotnet publish Src/Api/Expenso.Api/Expenso.Api.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app
COPY --from=builder /app/out .
ENV ASPNETCORE_URLS=http://*:8080;http://*:8081
EXPOSE 8080
EXPOSE 8081
ENTRYPOINT ["dotnet", "Expenso.Api.dll"]