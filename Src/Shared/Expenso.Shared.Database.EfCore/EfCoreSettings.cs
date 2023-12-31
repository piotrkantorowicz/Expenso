namespace Expenso.Shared.Database.EfCore;

public sealed record EfCoreSettings
{
    public string? ConnectionString { get; init; }

    public bool? InMemory { get; init; }
}