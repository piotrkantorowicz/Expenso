using Expenso.Api.Configuration.Extensions.Environment.Const;

namespace Expenso.Api.Configuration.Extensions.Environment;

internal static class EnvironmentExtensions
{
    public static bool IsTest(this IWebHostEnvironment environment)
    {
        return environment.IsEnvironment(environmentName: CustomEnvironments.Test);
    }

    public static bool IsLocal(this IWebHostEnvironment environment)
    {
        return environment.IsEnvironment(environmentName: CustomEnvironments.Local);
    }
}