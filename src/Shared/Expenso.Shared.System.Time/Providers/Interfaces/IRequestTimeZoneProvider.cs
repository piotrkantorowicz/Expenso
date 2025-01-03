using Microsoft.AspNetCore.Http;

namespace Expenso.Shared.System.Time.Providers.Interfaces;

public interface IRequestTimeZoneProvider
{
    Task<ProviderTimeZoneResult> DetermineProviderTimeZoneResult(HttpContext httpContext);
}