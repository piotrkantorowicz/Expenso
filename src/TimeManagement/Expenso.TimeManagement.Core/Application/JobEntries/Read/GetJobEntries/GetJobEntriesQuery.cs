using Expenso.Shared.Queries.Pagination;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.System.Types.Ordering;
using Expenso.Shared.System.Types.Paging;
using Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntries.DTO.Request;
using Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntries.DTO.Response;

namespace Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntries;

public sealed record GetJobEntriesQuery(
    IMessageContext MessageContext,
    Pagination? Pagination,
    Sorting? Sorters,
    GetJobEntriesRequest? Payload) : IPagedQuery<IPagedList<GetJobEntriesResponse>>;