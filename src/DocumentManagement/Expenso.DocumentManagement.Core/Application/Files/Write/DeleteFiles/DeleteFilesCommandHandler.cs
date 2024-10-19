using System.IO.Abstractions;

using Expenso.DocumentManagement.Core.Application.Shared.Models;
using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.DocumentManagement.Shared.DTO.API.DeleteFiles.Request;
using Expenso.Shared.Commands;

namespace Expenso.DocumentManagement.Core.Application.Files.Write.DeleteFiles;

internal sealed class DeleteFilesCommandHandler : ICommandHandler<DeleteFilesCommand>
{
    private readonly IDirectoryPathResolver _directoryPathResolver;
    private readonly IFileStorage _fileStorage;
    private readonly IFileSystem _fileSystem;

    public DeleteFilesCommandHandler(IFileStorage fileStorage, IDirectoryPathResolver directoryPathResolver,
        IFileSystem fileSystem)
    {
        _directoryPathResolver = directoryPathResolver ??
                                 throw new ArgumentNullException(paramName: nameof(directoryPathResolver));

        _fileStorage = fileStorage ?? throw new ArgumentNullException(paramName: nameof(fileStorage));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(paramName: nameof(fileSystem));
    }

    public async Task HandleAsync(DeleteFilesCommand command, CancellationToken cancellationToken)
    {
        string directoryPath = _directoryPathResolver.ResolvePath(
            fileType: (FileType)(command.Payload?.FileType ?? DeleteFilesRequest_FileType.None),
            userId: (command.Payload?.UserId ?? command.MessageContext.RequestedBy).ToString(),
            groups: command.Payload?.Groups);

        foreach (string fileName in command.Payload?.FileNames ?? [])
        {
            string fullFilePath = _fileSystem.Path.Combine(path1: directoryPath, path2: fileName);
            await _fileStorage.DeleteAsync(path: fullFilePath, cancellationToken: cancellationToken);
        }
    }
}