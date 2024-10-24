using System.IO.Abstractions;

using Expenso.Shared.System.Configuration.Settings.Files;

namespace Expenso.DocumentManagement.Core.Application.Shared.Services.Acl.Disk;

internal sealed class DirectoryInfoService : IDirectoryInfoService
{
    public const string Reports = "Reports";
    public const string Imports = "Imports";
    private readonly FilesSettings _filesSettings;
    private readonly IFileSystem _fileSystem;
    private readonly string _rootPath = Environment.GetFolderPath(folder: Environment.SpecialFolder.ApplicationData);

    public DirectoryInfoService(IFileSystem fileSystem, FilesSettings filesSettings)
    {
        _filesSettings = filesSettings ?? throw new ArgumentNullException(paramName: nameof(filesSettings));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(paramName: nameof(fileSystem));
    }

    public string GetReportsDirectory(string userId, string[]? groups, string date)
    {
        return _fileSystem.Path.Combine(paths:
        [
            _filesSettings.RootPath ?? _rootPath, userId, _filesSettings.ReportsDirectory ?? Reports, date,
            groups is null ? string.Empty : _fileSystem.Path.Combine(paths: groups)
        ]);
    }

    public string GetImportsDirectory(string userId, string[]? groups, string date)
    {
        return _fileSystem.Path.Combine(paths:
        [
            _filesSettings.RootPath ?? _rootPath, userId, _filesSettings.ImportDirectory ?? Imports, date,
            groups is null ? string.Empty : _fileSystem.Path.Combine(paths: groups)
        ]);
    }
}