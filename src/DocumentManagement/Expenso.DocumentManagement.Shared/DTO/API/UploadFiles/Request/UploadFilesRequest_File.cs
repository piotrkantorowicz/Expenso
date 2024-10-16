namespace Expenso.DocumentManagement.Shared.DTO.API.UploadFiles.Request;

public sealed record UploadFilesRequest_File(string? Name, byte[]? Content);