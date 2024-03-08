namespace Expenso.DocumentManagement.Proxy.DTO.API.GetFiles.Response;

public sealed record GetFilesResponse(
    string UserId,
    string FileName,
    byte[] FileContent,
    GetFilesResponse_FileType FilesResponseFileType);