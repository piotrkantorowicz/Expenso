using Expenso.Shared.System.Time.Request;

namespace Expenso.Shared.System.Time.Features.Interfaces;

public interface IRequestDateTimeFormatFeature : IRequestFeature
{
    public RequestDateTimeFormat RequestDateTimeFormat { get; }
}