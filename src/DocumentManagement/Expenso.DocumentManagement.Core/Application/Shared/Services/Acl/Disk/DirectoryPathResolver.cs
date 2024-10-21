using Expenso.DocumentManagement.Core.Application.Shared.Exceptions;
using Expenso.DocumentManagement.Core.Application.Shared.Models;
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

    public string ResolvePath(FileType fileType, string userId, string[]? groups)
    {
        string directoryPath = fileType switch
        {
            FileType.Import => _directoryInfoService.GetImportsDirectory(userId: userId, groups: groups,
                date: _clock.UtcNow.ToString(format: "yyyyMMdd")),
            FileType.Report => _directoryInfoService.GetReportsDirectory(userId: userId, groups: groups,
                date: _clock.UtcNow.ToString(format: "yyyyMMdd")),
            _ or FileType.None => throw new InvalidFileTypeException(typeName: fileType.ToString())
        };

        return directoryPath;
    }
}