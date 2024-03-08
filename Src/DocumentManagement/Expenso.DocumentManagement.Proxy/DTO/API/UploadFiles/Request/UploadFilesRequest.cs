namespace Expenso.DocumentManagement.Proxy.DTO.API.UploadFiles.Request;

public sealed record UploadFilesRequest(
    string? UserId,
    string[]? Groups,
    UploadFilesRequest_File[] Files,
    UploadFilesRequest_FileType FilesRequestFileType);