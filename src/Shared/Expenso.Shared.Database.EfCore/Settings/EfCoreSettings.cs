namespace Expenso.Shared.Database.EfCore.Settings;

public sealed record EfCoreSettings
{
    public ConnectionParameters? ConnectionParameters { get; init; }

    public bool? InMemory { get; init; }

    public bool? UseMigration { get; init; }

    public bool? UseSeeding { get; init; }
}