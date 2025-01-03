using Expenso.Shared.System.Time.Middleware;

using Microsoft.AspNetCore.Builder;

namespace Expenso.Shared.System.Time.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseRequestTimeZone(this IApplicationBuilder applicationBuilder)
    {
        ArgumentNullException.ThrowIfNull(argument: applicationBuilder);

        return applicationBuilder.UseMiddleware<RequestTimeZoneMiddleware>();
    }
}