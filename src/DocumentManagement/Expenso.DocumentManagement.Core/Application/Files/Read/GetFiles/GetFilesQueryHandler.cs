using System.IO.Abstractions;

using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.DocumentManagement.Shared.DTO.API.GetFiles.Request;
using Expenso.DocumentManagement.Shared.DTO.API.GetFiles.Response;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

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
        (IMessageContext messageContext,
            (Guid? userId, string[]? groups, string[] fileNames, GetFilesRequest_FileType fileType)) = query;

        userId ??= messageContext.RequestedBy;

        string directoryPath =
            _directoryPathResolver.ResolvePath(fileType: (int)fileType, userId: userId.ToString()!, groups: groups);

        List<GetFilesResponse> filesResponses = [];

        foreach (string fileName in fileNames)
        {
            string filePath = _fileSystem.Path.Combine(path1: directoryPath, path2: fileName);
            byte[] fileContent = await _fileStorage.ReadAsync(path: filePath, cancellationToken: cancellationToken);

            filesResponses.Add(item: new GetFilesResponse(UserId: userId.Value, FileName: fileName,
                FileContent: fileContent, FilesResponseFileType: (GetFilesResponse_FileType)fileType));
        }

        return filesResponses;
    }
}