using Expenso.Shared.System.Time.Providers.Inputs;

using Microsoft.AspNetCore.Http;

namespace Expenso.Shared.System.Time.Providers.Interfaces;

public interface IRequestProvider
{
    RequestProviderType ProviderType { get; }

    Task<ProviderResult> DetermineProviderResult(HttpContext httpContext, IProviderInput providerInput);
}