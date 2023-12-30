using Expenso.IAM.Core.ModuleApi;
using Expenso.IAM.Core.Services;
using Expenso.IAM.Core.Services.Interfaces;
using Expenso.IAM.Proxy;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.IAM.Core;

public static class RegistrationExtensions
{
    public static void AddIamCore(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IIamApi, IamApi>();
    }
}