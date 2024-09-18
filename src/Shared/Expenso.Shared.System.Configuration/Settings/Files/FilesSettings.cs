namespace Expenso.Shared.System.Configuration.Settings.Files;

public sealed record FilesSettings : ISettings
{
    public FileStorageType StorageType { get; init; }

    public string? RootPath { get; init; }

    public string? ImportDirectory { get; init; }

    public string? ReportsDirectory { get; init; }
}