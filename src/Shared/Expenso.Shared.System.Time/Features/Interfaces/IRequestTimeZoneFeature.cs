using Expenso.Shared.System.Time.Providers.Interfaces;
using Expenso.Shared.System.Time.Request;

namespace Expenso.Shared.System.Time.Features.Interfaces;

public interface IRequestTimeZoneFeature
{
    RequestTimeZone RequestTimeZone { get; }

    IRequestTimeZoneProvider? Provider { get; }
}