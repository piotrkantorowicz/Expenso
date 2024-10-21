using Expenso.DocumentManagement.Core.Application.Shared.Const;
using Expenso.DocumentManagement.Core.Application.Shared.Exceptions;
using Expenso.DocumentManagement.Core.Application.Shared.Models;
using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.DocumentManagement.Shared.DTO.API.UploadFiles.Request;
using Expenso.Shared.Commands;

namespace Expenso.DocumentManagement.Core.Application.Files.Write.UploadFiles;

internal sealed class UploadFilesCommandHandler : ICommandHandler<UploadFilesCommand>
{
    private readonly IDirectoryPathResolver _directoryPathResolver;
    private readonly IFileStorage _fileStorage;

    public UploadFilesCommandHandler(IFileStorage fileStorage, IDirectoryPathResolver directoryPathResolver)
    {
        _directoryPathResolver = directoryPathResolver ??
                                 throw new ArgumentNullException(paramName: nameof(directoryPathResolver));

        _fileStorage = fileStorage ?? throw new ArgumentNullException(paramName: nameof(fileStorage));
    }

    public async Task HandleAsync(UploadFilesCommand command, CancellationToken cancellationToken)
    {
        string directoryPath = _directoryPathResolver.ResolvePath(
            fileType: (FileType)(command.Payload?.FileType ?? UploadFilesRequest_FileType.None),
            userId: (command.Payload?.UserId ?? command.MessageContext.RequestedBy).ToString(),
            groups: command.Payload?.Groups);

        foreach (UploadFilesRequest_File file in command.Payload?.Files ?? [])
        {
            if (file.Content is null || file.Content.Length is 0)
            {
                throw new EmptyFileContentException();
            }

            string validatedFileName = file.Name ?? $"{Guid.NewGuid()}.{FileExtensions.Xlsx}";

            await _fileStorage.SaveAsync(directoryPath: directoryPath, fileName: validatedFileName,
                byteContent: file.Content, cancellationToken: cancellationToken);
        }
    }
}