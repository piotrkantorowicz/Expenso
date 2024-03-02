using System.Text.Json.Serialization;

using Expenso.Api.Configuration.Extensions.Environment.Const;
using Expenso.Api.Tests.E2E.BudgetSharing.Persistence;
using Expenso.Api.Tests.E2E.IAM;
using Expenso.IAM.Proxy;
using Expenso.Shared.Database;
using Expenso.Shared.Database.EfCore;
using Expenso.Shared.System.Configuration.Extensions;

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
        builder.UseEnvironment(Environment);

        builder.ConfigureTestServices(services =>
        {
            ConfigureTestSerializer(services);
            UseFakeIIamProxy(services);

            if (GetEfCoreSettings().InMemory == true)
            {
                UseFakeUnitOfWork(services);
            }

            services.AddAuthentication(FakeJwtBearerDefaults.AuthenticationScheme).AddFakeJwtBearer();
        });
    }

    private static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment}.json")
            .Build();
    }

    private static void UseFakeUnitOfWork(IServiceCollection services)
    {
        ServiceDescriptor unitOfWork = services.Single(d => d.ServiceType == typeof(IUnitOfWork));
        services.Remove(unitOfWork);
        services.AddScoped<IUnitOfWork, FakeUnitOfWork>();
    }

    private static void UseFakeIIamProxy(IServiceCollection services)
    {
        ServiceDescriptor iamProxy = services.Single(d => d.ServiceType == typeof(IIamProxy));
        services.Remove(iamProxy);
        services.AddScoped<IIamProxy, FakeIamProxy>();
    }

    private static void ConfigureTestSerializer(IServiceCollection services)
    {
        services.Configure<JsonOptions>(options =>
        {
            // Remove JsonStringEnumConverter to avoid serialization issues with enums
            options.SerializerOptions.Converters.Remove(options.SerializerOptions.Converters.Single(c =>
                c.GetType() == typeof(JsonStringEnumConverter)));
        });
    }

    private EfCoreSettings GetEfCoreSettings()
    {
        if (!_configuration.TryBindOptions("EfCore", out EfCoreSettings efCoreSettings))
        {
            throw new InvalidOperationException("EfCore settings not found.");
        }

        return efCoreSettings;
    }
}