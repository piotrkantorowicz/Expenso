using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.TimeManagement.Shared.DTO.GetJobEntry.Request;
using Expenso.TimeManagement.Shared.DTO.GetJobEntry.Response;

namespace Expenso.TimeManagement.Core.Application.Jobs.Read.GetJobEntry;

public sealed record GetJobEntryQuery(IMessageContext MessageContext, GetJobEntryRequest? Payload)
    : IQuery<GetJobEntryResponse>;