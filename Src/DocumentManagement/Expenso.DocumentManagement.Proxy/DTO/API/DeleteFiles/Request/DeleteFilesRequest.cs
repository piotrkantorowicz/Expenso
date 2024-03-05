namespace Expenso.DocumentManagement.Proxy.DTO.API.DeleteFiles.Request;

public sealed record DeleteFilesRequest(
    string? UserId,
    string[]? Groups,
    string[] FileNames,
    DeleteFilesRequest_FileType FilesRequestFileType);