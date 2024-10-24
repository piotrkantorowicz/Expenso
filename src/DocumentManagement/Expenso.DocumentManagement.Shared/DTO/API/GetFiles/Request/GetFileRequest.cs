﻿namespace Expenso.DocumentManagement.Shared.DTO.API.GetFiles.Request;

public sealed record GetFileRequest(
    Guid? UserId,
    string[]? Groups,
    string[] FileNames,
    GetFilesRequest_FileType FileType);