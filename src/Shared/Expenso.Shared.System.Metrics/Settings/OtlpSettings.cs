using Expenso.Shared.System.Configuration.Settings;

namespace Expenso.Shared.System.Metrics.Settings;

public sealed record OtlpSettings : ISettings
{
    public string? ServiceName { get; init; }

    public string? Endpoint { get; init; }
}