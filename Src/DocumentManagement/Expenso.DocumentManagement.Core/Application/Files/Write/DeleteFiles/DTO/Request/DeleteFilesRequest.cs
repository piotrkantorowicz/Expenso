namespace Expenso.DocumentManagement.Core.Application.Files.Write.DeleteFiles.DTO.Request;

public sealed record DeleteFilesRequest(
    string? UserId,
    string[]? Groups,
    string[] FileName,
    DeleteFilesRequest_FileType FilesRequestFileType);