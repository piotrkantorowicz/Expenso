namespace Expenso.Shared.System.Configuration.Settings.App;

public sealed record ApplicationSettings : ISettings
{
    public Guid? InstanceId { get; init; } = Guid.NewGuid();

    public string? Name { get; init; }

    public string? Version { get; init; }
}