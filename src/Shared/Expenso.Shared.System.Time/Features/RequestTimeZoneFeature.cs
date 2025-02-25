using Expenso.Shared.System.Time.Features.Interfaces;
using Expenso.Shared.System.Time.Providers.Interfaces;
using Expenso.Shared.System.Time.Request;

namespace Expenso.Shared.System.Time.Features;

internal sealed record RequestTimeZoneFeature : IRequestTimeZoneFeature
{
    public RequestTimeZoneFeature(RequestTimeZone requestTimeZone, IRequestProvider? provider = null)
    {
        RequestTimeZone = requestTimeZone ?? throw new ArgumentNullException(paramName: nameof(requestTimeZone));
        Provider = provider;
    }

    public RequestTimeZone RequestTimeZone { get; }

    public IRequestProvider? Provider { get; }
}