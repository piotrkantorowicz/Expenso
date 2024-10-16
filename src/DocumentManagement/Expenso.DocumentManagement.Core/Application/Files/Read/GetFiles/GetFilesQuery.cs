using Expenso.DocumentManagement.Shared.DTO.API.GetFiles.Request;
using Expenso.DocumentManagement.Shared.DTO.API.GetFiles.Response;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.DocumentManagement.Core.Application.Files.Read.GetFiles;

public sealed record GetFilesQuery(IMessageContext MessageContext, GetFileRequest GetFileRequest)
    : IQuery<IEnumerable<GetFilesResponse>>;