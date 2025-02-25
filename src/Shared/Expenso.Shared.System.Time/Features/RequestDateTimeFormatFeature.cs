using Expenso.Shared.System.Time.Features.Interfaces;
using Expenso.Shared.System.Time.Providers.Interfaces;
using Expenso.Shared.System.Time.Request;

namespace Expenso.Shared.System.Time.Features;

internal sealed class RequestDateTimeFormatFeature : IRequestDateTimeFormatFeature
{
    public RequestDateTimeFormatFeature(RequestDateTimeFormat requestDateTimeFormat, IRequestProvider? provider = null)
    {
        RequestDateTimeFormat = requestDateTimeFormat ??
                                throw new ArgumentNullException(paramName: nameof(requestDateTimeFormat));

        Provider = provider;
    }

    public RequestDateTimeFormat RequestDateTimeFormat { get; }

    public IRequestProvider? Provider { get; }
}