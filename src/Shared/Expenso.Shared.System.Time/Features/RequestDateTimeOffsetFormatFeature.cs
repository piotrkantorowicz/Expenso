using Expenso.Shared.System.Time.Features.Interfaces;
using Expenso.Shared.System.Time.Providers.Interfaces;
using Expenso.Shared.System.Time.Request;

namespace Expenso.Shared.System.Time.Features;

internal sealed class RequestDateTimeOffsetFormatFeature : IRequestDateTimeOffsetFormatFeature
{
    public RequestDateTimeOffsetFormatFeature(RequestDateTimeOffsetFormat requestDateTimeOffsetFormat,
        IRequestProvider? provider = null)
    {
        RequestDateTimeOffsetFormat = requestDateTimeOffsetFormat ??
                                      throw new ArgumentNullException(paramName: nameof(requestDateTimeOffsetFormat));

        Provider = provider;
    }

    public RequestDateTimeOffsetFormat RequestDateTimeOffsetFormat { get; }

    public IRequestProvider? Provider { get; }
}