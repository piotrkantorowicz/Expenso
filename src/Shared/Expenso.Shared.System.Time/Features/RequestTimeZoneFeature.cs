using Expenso.Shared.System.Time.Features.Interfaces;
using Expenso.Shared.System.Time.Providers.Interfaces;
using Expenso.Shared.System.Time.Request;

namespace Expenso.Shared.System.Time.Features;

internal sealed class RequestTimeZoneFeature : IRequestTimeZoneFeature
{
    public RequestTimeZoneFeature(RequestTimeZone requestTimeZone, IRequestTimeZoneProvider? provider = null)
    {
        RequestTimeZone = requestTimeZone ?? throw new ArgumentNullException(paramName: nameof(requestTimeZone));
        Provider = provider;
    }

    public RequestTimeZone RequestTimeZone { get; }

    public IRequestTimeZoneProvider? Provider { get; }
}