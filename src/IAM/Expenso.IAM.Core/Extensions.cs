using Expenso.IAM.Core.Application.Users.Read.Services;
using Expenso.IAM.Core.Application.Users.Read.Services.Acl.Keycloak;
using Expenso.Shared.System.Configuration;
using Expenso.Shared.System.Configuration.Sections;
using Expenso.Shared.System.Configuration.Settings.Auth;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.IAM.Core;

public static class Extensions
{
    public static void AddIamCore(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterAclUserServices(services: services, configuration: configuration);
    }

    private static void RegisterAclUserServices(IServiceCollection services, IConfiguration configuration)
    {
        configuration.TryBindOptions(sectionName: SectionNames.Auth, options: out AuthSettings? authSettings);

        switch (authSettings?.AuthServer)
        {
            case AuthServer.Keycloak:
                services.AddScoped<IUserService, UserService>();

                break;
            default:
                throw new ArgumentOutOfRangeException(paramName: authSettings?.AuthServer.GetType().Name,
                    actualValue: authSettings?.AuthServer, message: "Invalid auth server type");
        }
    }
}