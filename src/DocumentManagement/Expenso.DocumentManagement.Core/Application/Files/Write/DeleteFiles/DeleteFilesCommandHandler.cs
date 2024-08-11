using System.IO.Abstractions;

using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.DocumentManagement.Proxy.DTO.API.DeleteFiles.Request;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.DocumentManagement.Core.Application.Files.Write.DeleteFiles;

internal sealed class DeleteFilesCommandHandler(
    IFileStorage fileStorage,
    IDirectoryPathResolver directoryPathResolver,
    IFileSystem fileSystem) : ICommandHandler<DeleteFilesCommand>
{
    private readonly IDirectoryPathResolver _directoryPathResolver = directoryPathResolver ??
                                                                     throw new ArgumentNullException(
                                                                         paramName: nameof(directoryPathResolver));

    private readonly IFileStorage _fileStorage =
        fileStorage ?? throw new ArgumentNullException(paramName: nameof(fileStorage));

    private readonly IFileSystem _fileSystem =
        fileSystem ?? throw new ArgumentNullException(paramName: nameof(fileSystem));

    public async Task HandleAsync(DeleteFilesCommand command, CancellationToken cancellationToken)
    {
        (IMessageContext messageContext,
            (Guid? userId, string[]? groups, string[] fileNames, DeleteFilesRequest_FileType fileType)) = command;

        string directoryPath = _directoryPathResolver.ResolvePath(fileType: (int)fileType,
            userId: (userId ?? messageContext.RequestedBy).ToString(), groups: groups);

        foreach (string fileName in fileNames)
        {
            string fullFilePath = _fileSystem.Path.Combine(path1: directoryPath, path2: fileName);
            await _fileStorage.DeleteAsync(path: fullFilePath, cancellationToken: cancellationToken);
        }
    }
}