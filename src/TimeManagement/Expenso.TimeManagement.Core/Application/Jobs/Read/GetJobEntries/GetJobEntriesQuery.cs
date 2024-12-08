using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.TimeManagement.Core.Application.Jobs.Read.GetJobEntries.DTO.Request;
using Expenso.TimeManagement.Core.Application.Jobs.Read.GetJobEntries.DTO.Response;

namespace Expenso.TimeManagement.Core.Application.Jobs.Read.GetJobEntries;

public sealed record GetJobEntriesQuery(IMessageContext MessageContext, GetJobEntriesRequest? Payload)
    : IQuery<IReadOnlyCollection<GetJobEntriesResponse>>;