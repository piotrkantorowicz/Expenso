using Expenso.Shared.System.Time.Features.Interfaces;

using Microsoft.AspNetCore.Http;

namespace Expenso.Shared.System.Time.Extensions;

public static class HttpContextExtensions
{
    public static TimeZoneInfo GetUserTimeZone(this HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(argument: httpContext);

        return httpContext.Features.Get<IRequestTimeZoneFeature>()?.RequestTimeZone.TimeZone ?? TimeZoneInfo.Utc;
    }
}