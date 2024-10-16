namespace Expenso.DocumentManagement.Shared.DTO.API.GetFiles.Response;

public sealed record GetFilesResponse(
    Guid? UserId,
    string FileName,
    byte[] FileContent,
    GetFilesResponse_FileType FilesResponseFileType);