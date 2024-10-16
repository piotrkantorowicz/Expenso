namespace Expenso.DocumentManagement.Shared.DTO.API.UploadFiles.Request;

public sealed record UploadFilesRequest(
    Guid? UserId,
    string[]? Groups,
    UploadFilesRequest_File[] Files,
    UploadFilesRequest_FileType FileType);