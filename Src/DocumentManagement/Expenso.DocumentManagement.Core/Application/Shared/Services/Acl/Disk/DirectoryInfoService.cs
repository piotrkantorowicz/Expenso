using System.IO.Abstractions;

using Expenso.Shared.System.Configuration.Settings.Files;

namespace Expenso.DocumentManagement.Core.Application.Shared.Services.Acl.Disk;

internal sealed class DirectoryInfoService(IFileSystem fileSystem, FilesSettings filesSettings) : IDirectoryInfoService
{
    private const string Reports = "Reports";
    private const string Imports = "Imports";

    private readonly FilesSettings _filesSettings =
        filesSettings ?? throw new ArgumentNullException(nameof(filesSettings));

    private readonly IFileSystem _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    private readonly string _rootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

    public string GetReportsDirectory(string userId, string[]? groups, string date)
    {
        return _fileSystem.Path.Combine([
            _filesSettings.RootPath ?? _rootPath, userId, groups is null ? string.Empty : Path.Combine(groups), date,
            _filesSettings.ReportsDirectory ?? Reports
        ]);
    }

    public string GetImportsDirectory(string userId, string[]? groups, string date)
    {
        return _fileSystem.Path.Combine([
            _filesSettings.RootPath ?? _rootPath, userId, groups is null ? string.Empty : Path.Combine(groups), date,
            _filesSettings.ImportDirectory ?? Imports
        ]);
    }
}