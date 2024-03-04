using Expenso.Shared.System.Types.Clock;

namespace Expenso.DocumentManagement.Core.Application.Shared.Services.Acl.Disk;

internal class DirectoryPathResolver(IDirectoryInfoService directoryInfoService, IClock clock) : IDirectoryPathResolver
{
    private readonly IDirectoryInfoService _directoryInfoService =
        directoryInfoService ?? throw new ArgumentNullException(nameof(directoryInfoService));

    public string ResolvePath(int fileType, string userId, string[]? groups)
    {
        string directoryPath = fileType switch
        {
            1 => _directoryInfoService.GetImportsDirectory(userId, groups, clock.UtcNow.ToString("yyyyMMdd")),
            2 => _directoryInfoService.GetReportsDirectory(userId, groups, clock.UtcNow.ToString("yyyyMMdd")),
            _ => throw new ArgumentOutOfRangeException(nameof(fileType), fileType, "Unknown file type.")
        };

        return directoryPath;
    }
}