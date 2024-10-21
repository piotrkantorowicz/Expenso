using System.IO.Abstractions;

using Expenso.DocumentManagement.Core.Application.Shared.Models;
using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.DocumentManagement.Shared.DTO.API.GetFiles.Request;
using Expenso.DocumentManagement.Shared.DTO.API.GetFiles.Response;
using Expenso.Shared.Queries;

namespace Expenso.DocumentManagement.Core.Application.Files.Read.GetFiles;

internal sealed class GetFilesQueryHandler : IQueryHandler<GetFilesQuery, IEnumerable<GetFilesResponse>>
{
    private readonly IDirectoryPathResolver _directoryPathResolver;
    private readonly IFileStorage _fileStorage;
    private readonly IFileSystem _fileSystem;

    public GetFilesQueryHandler(IDirectoryPathResolver directoryPathResolver, IFileStorage fileStorage,
        IFileSystem fileSystem)
    {
        _directoryPathResolver = directoryPathResolver ??
                                 throw new ArgumentNullException(paramName: nameof(directoryPathResolver));

        _fileStorage = fileStorage ?? throw new ArgumentNullException(paramName: nameof(fileStorage));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(paramName: nameof(fileSystem));
    }

    public async Task<IEnumerable<GetFilesResponse>?> HandleAsync(GetFilesQuery query,
        CancellationToken cancellationToken)
    {
        Guid userId = query.Payload?.UserId ?? query.MessageContext.RequestedBy;

        string directoryPath = _directoryPathResolver.ResolvePath(
            fileType: (FileType)(query.Payload?.FileType ?? GetFilesRequest_FileType.None), userId: userId.ToString(),
            groups: query.Payload?.Groups);

        List<GetFilesResponse> filesResponses = [];

        foreach (string fileName in query.Payload?.FileNames ?? [])
        {
            string filePath = _fileSystem.Path.Combine(path1: directoryPath, path2: fileName);
            byte[] fileContent = await _fileStorage.ReadAsync(path: filePath, cancellationToken: cancellationToken);

            filesResponses.Add(item: new GetFilesResponse(UserId: userId, FileName: fileName, FileContent: fileContent,
                FilesResponseFileType: (GetFilesResponse_FileType)(query.Payload?.FileType ??
                                                                   GetFilesRequest_FileType.None)));
        }

        return filesResponses;
    }
}