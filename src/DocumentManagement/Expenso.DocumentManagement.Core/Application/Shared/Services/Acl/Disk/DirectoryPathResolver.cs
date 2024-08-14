using Expenso.Shared.System.Types.Clock;

namespace Expenso.DocumentManagement.Core.Application.Shared.Services.Acl.Disk;

internal sealed class DirectoryPathResolver : IDirectoryPathResolver
{
    private readonly IClock _clock;
    private readonly IDirectoryInfoService _directoryInfoService;

    public DirectoryPathResolver(IDirectoryInfoService directoryInfoService, IClock clock)
    {
        _clock = clock;

        _directoryInfoService = directoryInfoService ??
                                throw new ArgumentNullException(paramName: nameof(directoryInfoService));
    }

    public string ResolvePath(int fileType, string userId, string[]? groups)
    {
        string directoryPath = fileType switch
        {
            1 => _directoryInfoService.GetImportsDirectory(userId: userId, groups: groups,
                date: _clock.UtcNow.ToString(format: "yyyyMMdd")),
            2 => _directoryInfoService.GetReportsDirectory(userId: userId, groups: groups,
                date: _clock.UtcNow.ToString(format: "yyyyMMdd")),
            _ => throw new ArgumentOutOfRangeException(paramName: nameof(fileType), actualValue: fileType,
                message: "Unknown file type")
        };

        return directoryPath;
    }
}