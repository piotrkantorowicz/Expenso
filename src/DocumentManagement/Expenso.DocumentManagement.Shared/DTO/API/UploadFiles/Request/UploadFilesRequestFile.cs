namespace Expenso.DocumentManagement.Shared.DTO.API.UploadFiles.Request;

public sealed record UploadFilesRequestFile(string? Name, byte[]? Content);