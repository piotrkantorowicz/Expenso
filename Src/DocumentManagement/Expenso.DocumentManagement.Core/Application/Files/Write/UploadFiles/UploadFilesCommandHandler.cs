using Expenso.DocumentManagement.Core.Application.Shared.Const;
using Expenso.DocumentManagement.Core.Application.Shared.Exceptions;
using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.DocumentManagement.Proxy.DTO.API.UploadFiles.Request;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

using FileSignatures;

namespace Expenso.DocumentManagement.Core.Application.Files.Write.UploadFiles;

internal sealed class UploadFilesCommandHandler(
    IFileStorage fileStorage,
    IDirectoryPathResolver directoryPathResolver,
    IFileFormatInspector fileFormatInspector) : ICommandHandler<UploadFilesCommand>
{
    private readonly IDirectoryPathResolver _directoryPathResolver =
        directoryPathResolver ?? throw new ArgumentNullException(nameof(directoryPathResolver));

    private readonly IFileFormatInspector _fileFormatInspector =
        fileFormatInspector ?? throw new ArgumentNullException(nameof(fileFormatInspector));

    private readonly IFileStorage _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));

    public async Task HandleAsync(UploadFilesCommand command, CancellationToken cancellationToken)
    {
        (IMessageContext messageContext,
            (string? userId, string[]? groups, UploadFilesRequest_File[] fileContents,
                UploadFilesRequest_FileType fileType)) = command;

        string directoryPath =
            _directoryPathResolver.ResolvePath((int)fileType, userId ?? messageContext.RequestedBy.ToString(), groups);

        foreach (UploadFilesRequest_File file in fileContents)
        {
            FileFormat? fileFormat;

            if (file.Content is null || file.Content.Length == 0)
            {
                throw new EmptyFileContentException();
            }

            using (MemoryStream steam = new(file.Content))
            {
                fileFormat = _fileFormatInspector.DetermineFileFormat(steam);
            }

            if (!FileExtensions.SupportedExtensions.Select(x => x.ToLower()).Contains(fileFormat?.Extension.ToLower()))
            {
                throw new UnsupportedFileExtensionException(fileFormat?.Extension);
            }

            string validatedFileName = file.Name ?? $"{Guid.NewGuid()}{fileFormat?.Extension}";
            await _fileStorage.SaveAsync(directoryPath, validatedFileName, file.Content, cancellationToken);
        }
    }
}