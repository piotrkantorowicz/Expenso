using Expenso.IAM.Core.Proxy;
using Expenso.IAM.Core.Services;
using Expenso.IAM.Core.Services.KeycloakAcl;
using Expenso.IAM.Proxy;
using Expenso.Shared.Configuration.Extensions;
using Expenso.Shared.Configuration.Sections;
using Expenso.Shared.Configuration.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.IAM.Core;

public static class RegistrationExtensions
{
    public static void AddIamCore(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterAclUserServices(services, configuration);
        services.AddScoped<IIamProxy, IamProxy>();
    }

    private static void RegisterAclUserServices(IServiceCollection services, IConfiguration configuration)
    {
        configuration.TryBindOptions(SectionNames.ApplicationSection, out ApplicationSettings? applicationSettings);

        switch (applicationSettings?.AuthServer)
        {
            case AuthServer.Keycloak:
                services.AddScoped<IUserService, KeycloakAclUserService>();

                break;
            default:
                throw new ArgumentOutOfRangeException(applicationSettings?.AuthServer.GetType().Name,
                    applicationSettings?.AuthServer, "Invalid auth server type.");
        }
    }
}