namespace Expenso.DocumentManagement.Shared.DTO.API.UploadFiles.Request;

public sealed record UploadFilesRequest(
    Guid? UserId,
    string[]? Groups,
    UploadFilesRequestFile[] Files,
    UploadFilesRequestFileType FileType);