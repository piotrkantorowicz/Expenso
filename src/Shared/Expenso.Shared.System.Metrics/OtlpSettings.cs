namespace Expenso.Shared.System.Metrics;

public sealed record OtlpSettings
{
    public string? ServiceName { get; init; }

    public string? Endpoint { get; init; }
}