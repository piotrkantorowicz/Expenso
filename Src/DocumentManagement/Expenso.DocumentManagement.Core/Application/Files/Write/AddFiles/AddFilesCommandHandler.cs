using Expenso.DocumentManagement.Core.Application.Files.Write.AddFiles.DTO.Request;
using Expenso.DocumentManagement.Core.Application.Shared.Const;
using Expenso.DocumentManagement.Core.Application.Shared.Exceptions;
using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

using FileSignatures;

namespace Expenso.DocumentManagement.Core.Application.Files.Write.AddFiles;

internal sealed class AddFilesCommandHandler(
    IFileStorage fileStorage,
    IDirectoryPathResolver directoryPathResolver,
    IFileFormatInspector fileFormatInspector) : ICommandHandler<AddFilesCommand>
{
    private readonly IDirectoryPathResolver _directoryPathResolver =
        directoryPathResolver ?? throw new ArgumentNullException(nameof(directoryPathResolver));

    private readonly IFileFormatInspector _fileFormatInspector =
        fileFormatInspector ?? throw new ArgumentNullException(nameof(fileFormatInspector));

    private readonly IFileStorage _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));

    public async Task HandleAsync(AddFilesCommand command, CancellationToken cancellationToken)
    {
        (IMessageContext messageContext,
            (string? userId, string[]? groups, IEnumerable<(string?, byte[])> fileContents,
                AddFilesRequest_FileType fileType)) = command;

        string directoryPath =
            _directoryPathResolver.ResolvePath((int)fileType, userId ?? messageContext.RequestedBy.ToString(), groups);

        foreach ((string? fileName, byte[] fileContent) in fileContents)
        {
            FileFormat? fileFormat;

            using (MemoryStream steam = new(fileContent))
            {
                fileFormat = _fileFormatInspector.DetermineFileFormat(steam);
            }

            if (FileExtensions.SupportedExtensions.Contains(fileFormat?.Extension))
            {
                throw new UnsupportedFileExtensionException(fileFormat?.Extension);
            }

            string validatedFileName = fileName ?? $"{Guid.NewGuid()}{fileFormat?.Extension}";
            await _fileStorage.SaveAsync(directoryPath, validatedFileName, fileContent, cancellationToken);
        }
    }
}