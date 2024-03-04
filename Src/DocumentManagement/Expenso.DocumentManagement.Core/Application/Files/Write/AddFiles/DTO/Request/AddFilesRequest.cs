namespace Expenso.DocumentManagement.Core.Application.Files.Write.AddFiles.DTO.Request;

public sealed record AddFilesRequest(
    string? UserId,
    string[]? Groups,
    IEnumerable<(string?, byte[])> Files,
    AddFilesRequest_FileType FilesRequestFileType);