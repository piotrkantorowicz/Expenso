using System.IO.Abstractions;

using Expenso.DocumentManagement.Core.Application.Files.Write.DeleteFiles.DTO.Request;
using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.DocumentManagement.Core.Application.Files.Write.DeleteFiles;

internal sealed class DeleteFilesCommandHandler(
    IFileStorage fileStorage,
    IDirectoryPathResolver directoryPathResolver,
    IFileSystem fileSystem) : ICommandHandler<DeleteFilesCommand>
{
    private readonly IDirectoryPathResolver _directoryPathResolver =
        directoryPathResolver ?? throw new ArgumentNullException(nameof(directoryPathResolver));

    private readonly IFileStorage _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
    private readonly IFileSystem _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));

    public async Task HandleAsync(DeleteFilesCommand command, CancellationToken cancellationToken)
    {
        (IMessageContext messageContext,
            (string? userId, string[]? groups, string[] fileNames, DeleteFilesRequest_FileType fileType)) = command;

        string directoryPath =
            _directoryPathResolver.ResolvePath((int)fileType, userId ?? messageContext.RequestedBy.ToString(), groups);

        foreach (string fileName in fileNames)
        {
            string fullFilePath = _fileSystem.Path.Combine(directoryPath, fileName);
            await _fileStorage.DeleteAsync(fullFilePath, cancellationToken);
        }
    }
}