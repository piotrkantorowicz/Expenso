using Expenso.Shared.System.Time.Constants;
using Expenso.Shared.System.Time.Providers;
using Expenso.Shared.System.Time.Providers.Inputs;
using Expenso.Shared.System.Time.Providers.Interfaces;

namespace Expenso.Shared.System.Time.Request.Settings;

public sealed record RequestTimeZoneOptions
{
    private readonly RequestTimeZone _defaultRequestTimeZone = new(name: TimeZoneIds.Utc);

    public RequestTimeZone DefaultRequestTimeZone => string.IsNullOrEmpty(value: Id)
        ? _defaultRequestTimeZone
        : new RequestTimeZone(name: Id);

    public string? Id { get; set; }

    public bool EnableRequestToUtc { get; set; }

    public bool EnableResponseToLocal { get; set; }

    public MvcOptionType MvcOptionType { get; set; } = MvcOptionType.All;

    public string DateTimeFormat { get; set; } = DateTimeFormats.Iso8601;

    public string DateTimeOffsetFormat { get; set; } = DateTimeFormats.Iso8601TimeZone;

    public string[] SupportedDateTimeFormats { get; set; } = [DateTimeFormats.Iso8601];

    public string[] SupportedDateTimeOffsetFormats { get; set; } = [DateTimeFormats.Iso8601TimeZone];

    public IList<IRequestProvider> RequestProviders { get; set; } =
    [
        new RequestQueryStringProvider(),
        new RequestHeaderProvider(),
        new RequestCookieProvider()
    ];

    internal string GetDefaultProviderValue(IProviderInput providerInput, RequestProviderType providerType)
    {
        return GetDefaultValue(providerInput: providerInput, providerType: providerType);
    }

    private string GetDefaultValue(IProviderInput providerInput, RequestProviderType providerType)
    {
        IRequestProvider? provider = RequestProviders.FirstOrDefault(predicate: x => x.ProviderType == providerType);

        return providerInput.GetInput(requestProviderType: provider?.ProviderType ?? RequestProviderType.None).Key;
    }
}