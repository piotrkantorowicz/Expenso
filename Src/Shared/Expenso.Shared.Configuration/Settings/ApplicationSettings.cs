namespace Expenso.Shared.Configuration.Settings;

public sealed record ApplicationSettings
{
    public Guid InstanceId { get; init; } = Guid.NewGuid();

    public string? Name { get; init; }

    public string? Version { get; init; }

    public AuthServer AuthServer { get; init; }
}