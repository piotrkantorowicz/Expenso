using System.IO.Abstractions;

using Expenso.Shared.System.Configuration.Settings.Files;

namespace Expenso.DocumentManagement.Core.Application.Shared.Services.Acl.Disk;

internal sealed class DirectoryInfoService(IFileSystem fileSystem, FilesSettings filesSettings) : IDirectoryInfoService
{
    public const string Reports = "Reports";
    public const string Imports = "Imports";

    private readonly FilesSettings _filesSettings =
        filesSettings ?? throw new ArgumentNullException(nameof(filesSettings));

    private readonly IFileSystem _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    private readonly string _rootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

    public string GetReportsDirectory(string userId, string[]? groups, string date)
    {
        return _fileSystem.Path.Combine([
            _filesSettings.RootPath ?? _rootPath, userId, _filesSettings.ReportsDirectory ?? Reports, date,
            groups is null ? string.Empty : _fileSystem.Path.Combine(groups)
        ]);
    }

    public string GetImportsDirectory(string userId, string[]? groups, string date)
    {
        return _fileSystem.Path.Combine([
            _filesSettings.RootPath ?? _rootPath, userId, _filesSettings.ImportDirectory ?? Imports, date,
            groups is null ? string.Empty : _fileSystem.Path.Combine(groups)
        ]);
    }
}