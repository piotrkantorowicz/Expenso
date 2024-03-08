using System.IO.Abstractions;

using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.DocumentManagement.Proxy.DTO.API.GetFiles.Request;
using Expenso.DocumentManagement.Proxy.DTO.API.GetFiles.Response;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.DocumentManagement.Core.Application.Files.Read.GetFiles;

internal sealed class GetFilesQueryHandler(
    IDirectoryPathResolver directoryPathResolver,
    IFileStorage fileStorage,
    IFileSystem fileSystem) : IQueryHandler<GetFilesQuery, IEnumerable<GetFilesResponse>>
{
    private readonly IDirectoryPathResolver _directoryPathResolver =
        directoryPathResolver ?? throw new ArgumentNullException(nameof(directoryPathResolver));

    private readonly IFileStorage _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
    private readonly IFileSystem _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));

    public async Task<IEnumerable<GetFilesResponse>?> HandleAsync(GetFilesQuery query,
        CancellationToken cancellationToken)
    {
        (IMessageContext messageContext, string? userId, string[]? groups, string[] fileNames,
            GetFilesRequest_FileType fileType) = query;

        userId ??= messageContext.RequestedBy.ToString();
        string directoryPath = _directoryPathResolver.ResolvePath((int)fileType, userId, groups);
        List<GetFilesResponse> filesResponses = [];

        foreach (string fileName in fileNames)
        {
            string filePath = _fileSystem.Path.Combine(directoryPath, fileName);
            byte[] fileContent = await _fileStorage.ReadAsync(filePath, cancellationToken);

            filesResponses.Add(new GetFilesResponse(userId, fileName, fileContent,
                (GetFilesResponse_FileType)fileType));
        }

        return filesResponses;
    }
}