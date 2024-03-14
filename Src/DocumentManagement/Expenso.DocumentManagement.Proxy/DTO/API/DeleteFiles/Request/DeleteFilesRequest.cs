namespace Expenso.DocumentManagement.Proxy.DTO.API.DeleteFiles.Request;

public sealed record DeleteFilesRequest(
    Guid? UserId,
    string[]? Groups,
    string[] FileNames,
    DeleteFilesRequest_FileType FileType);