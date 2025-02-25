using Expenso.Shared.System.Time.Request;

namespace Expenso.Shared.System.Time.Features.Interfaces;

public interface IRequestDateTimeOffsetFormatFeature : IRequestFeature
{
    public RequestDateTimeOffsetFormat RequestDateTimeOffsetFormat { get; }
}