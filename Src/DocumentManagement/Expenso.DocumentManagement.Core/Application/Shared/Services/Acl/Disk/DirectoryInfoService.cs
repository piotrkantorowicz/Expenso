using System.IO.Abstractions;

namespace Expenso.DocumentManagement.Core.Application.Shared.Services.Acl.Disk;

internal class DirectoryInfoService(string rootDirectory, IFileSystem fileSystem) : IDirectoryInfoService
{
    private const string Reports = "Reports";
    private const string Imports = "Imports";
    private readonly IFileSystem _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    private readonly string _rootDirectory = rootDirectory ?? throw new ArgumentNullException(nameof(rootDirectory));

    public string GetReportsDirectory(string userId, string[]? groups, string date)
    {
        return _fileSystem.Path.Combine([
            rootDirectory, userId, groups is null ? string.Empty : Path.Combine(groups), date, Reports
        ]);
    }

    public string GetImportsDirectory(string userId, string[]? groups, string date)
    {
        return _fileSystem.Path.Combine([
            _rootDirectory, userId, groups is null ? string.Empty : Path.Combine(groups), date, Imports
        ]);
    }
}