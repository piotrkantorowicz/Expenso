using System.Text.Json.Serialization;

using Expenso.Api.Configuration.Extensions.Environment.Const;
using Expenso.Api.Tests.E2E.BudgetSharing.Persistence;
using Expenso.Api.Tests.E2E.IAM;
using Expenso.IAM.Shared;
using Expenso.Shared.Database;
using Expenso.Shared.Database.EfCore.Settings;
using Expenso.Shared.System.Configuration;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using WebMotions.Fake.Authentication.JwtBearer;

namespace Expenso.Api.Tests.E2E.Configuration;

internal sealed class ExpensoWebApplication : WebApplicationFactory<Program>
{
    private const string Environment = CustomEnvironments.Test;
    private readonly IConfiguration _configuration = GetConfiguration();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(environment: Environment);

        builder.ConfigureTestServices(servicesConfiguration: services =>
        {
            ConfigureTestSerializer(services: services);
            UseFakeIIamProxy(services: services);
            EfCoreSettings efCoreSettings = GetEfCoreSettings();

            if (efCoreSettings.InMemory is true)
            {
                UseFakeUnitOfWork(services: services);
            }

            services.AddAuthentication(defaultScheme: FakeJwtBearerDefaults.AuthenticationScheme).AddFakeJwtBearer();
        });
    }

    private static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile(path: "appsettings.json")
            .AddJsonFile(path: $"appsettings.{Environment}.json")
            .Build();
    }

    private static void UseFakeUnitOfWork(IServiceCollection services)
    {
        ServiceDescriptor unitOfWork = services.Single(predicate: d => d.ServiceType == typeof(IUnitOfWork));
        services.Remove(item: unitOfWork);
        services.AddScoped<IUnitOfWork, FakeUnitOfWork>();
    }

    private static void UseFakeIIamProxy(IServiceCollection services)
    {
        ServiceDescriptor iamProxy = services.Single(predicate: d => d.ServiceType == typeof(IIamProxy));
        services.Remove(item: iamProxy);
        services.AddScoped<IIamProxy, FakeIamProxy>();
    }

    private static void ConfigureTestSerializer(IServiceCollection services)
    {
        services.Configure<JsonOptions>(configureOptions: options =>
        {
            // Remove JsonStringEnumConverter to avoid serialization issues with enums
            options.SerializerOptions.Converters.Remove(
                item: options.SerializerOptions.Converters.Single(predicate: c =>
                    c.GetType() == typeof(JsonStringEnumConverter)));
        });
    }

    private EfCoreSettings GetEfCoreSettings()
    {
        if (!_configuration.TryBindOptions(sectionName: "EfCore", options: out EfCoreSettings efCoreSettings))
        {
            throw new InvalidOperationException(message: "EfCore settings not found");
        }

        return efCoreSettings;
    }
}