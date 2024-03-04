using Expenso.DocumentManagement.Core.Application.Files.Read.GetFiles.DTO.Request;
using Expenso.DocumentManagement.Core.Application.Files.Read.GetFiles.DTO.Response;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.DocumentManagement.Core.Application.Files.Read.GetFiles;

public sealed record GetFilesQuery(
    IMessageContext MessageContext,
    string? UserId,
    string[]? Groups,
    string[] FileNames,
    GetFilesRequest_FileType FileType) : IQuery<IEnumerable<GetFilesResponse>>;