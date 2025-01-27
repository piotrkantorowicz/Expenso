namespace Expenso.Shared.System.Configuration.Settings.App;

public sealed record ApplicationSettings : ISettings
{
    public Guid? InstanceId { get; init; } = Guid.CreateVersion7();

    public string? Name { get; init; }

    public string? Version { get; init; }
}