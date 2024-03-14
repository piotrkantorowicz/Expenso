using Expenso.IAM.Core.Application.Proxy;
using Expenso.IAM.Core.Application.Users.Read.Services;
using Expenso.IAM.Core.Application.Users.Read.Services.Acl.Keycloak;
using Expenso.IAM.Proxy;
using Expenso.Shared.System.Configuration.Extensions;
using Expenso.Shared.System.Configuration.Sections;
using Expenso.Shared.System.Configuration.Settings.Auth;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.IAM.Core;

public static class Extensions
{
    public static void AddIamCore(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterAclUserServices(services, configuration);
        services.AddScoped<IIamProxy, IamProxy>();
    }

    private static void RegisterAclUserServices(IServiceCollection services, IConfiguration configuration)
    {
        configuration.TryBindOptions(SectionNames.Auth, out AuthSettings? authSettings);

        switch (authSettings?.AuthServer)
        {
            case AuthServer.Keycloak:
                services.AddScoped<IUserService, UserService>();

                break;
            default:
                throw new ArgumentOutOfRangeException(authSettings?.AuthServer.GetType().Name, authSettings?.AuthServer,
                    "Invalid auth server type.");
        }
    }
}