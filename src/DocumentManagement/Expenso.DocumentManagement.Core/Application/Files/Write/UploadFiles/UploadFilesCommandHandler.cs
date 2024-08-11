using Expenso.DocumentManagement.Core.Application.Shared.Const;
using Expenso.DocumentManagement.Core.Application.Shared.Exceptions;
using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.DocumentManagement.Proxy.DTO.API.UploadFiles.Request;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.DocumentManagement.Core.Application.Files.Write.UploadFiles;

internal sealed class UploadFilesCommandHandler(IFileStorage fileStorage, IDirectoryPathResolver directoryPathResolver)
    : ICommandHandler<UploadFilesCommand>
{
    private readonly IDirectoryPathResolver _directoryPathResolver = directoryPathResolver ??
                                                                     throw new ArgumentNullException(
                                                                         paramName: nameof(directoryPathResolver));

    private readonly IFileStorage _fileStorage =
        fileStorage ?? throw new ArgumentNullException(paramName: nameof(fileStorage));

    public async Task HandleAsync(UploadFilesCommand command, CancellationToken cancellationToken)
    {
        (IMessageContext messageContext,
            (Guid? userId, string[]? groups, UploadFilesRequest_File[] fileContents,
                UploadFilesRequest_FileType fileType)) = command;

        string directoryPath = _directoryPathResolver.ResolvePath(fileType: (int)fileType,
            userId: (userId ?? messageContext.RequestedBy).ToString(), groups: groups);

        foreach (UploadFilesRequest_File file in fileContents)
        {
            if (file.Content is null || file.Content.Length == 0)
            {
                throw new EmptyFileContentException();
            }

            string validatedFileName = file.Name ?? $"{Guid.NewGuid()}.{FileExtensions.Xlsx}";

            await _fileStorage.SaveAsync(directoryPath: directoryPath, fileName: validatedFileName,
                byteContent: file.Content, cancellationToken: cancellationToken);
        }
    }
}