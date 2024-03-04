namespace Expenso.DocumentManagement.Core.Application.Files.Read.GetFiles.DTO.Response;

public record GetFilesResponse(
    string UserId,
    string FileName,
    byte[] FileContent,
    GetFilesResponse_FileType FilesResponseFileType);