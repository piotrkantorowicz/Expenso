using Expenso.Shared.System.Time.Request;

namespace Expenso.Shared.System.Time.Features.Interfaces;

public interface IRequestTimeZoneFeature : IRequestFeature
{
    RequestTimeZone RequestTimeZone { get; }
}