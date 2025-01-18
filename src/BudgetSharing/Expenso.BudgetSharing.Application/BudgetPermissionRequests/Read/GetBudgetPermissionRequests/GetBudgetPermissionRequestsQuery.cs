using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Response;
using Expenso.Shared.Queries.Pagination;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.System.Types.Ordering;
using Expenso.Shared.System.Types.Paging;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests;

public sealed record GetBudgetPermissionRequestsQuery(
    IMessageContext MessageContext,
    Pagination? Pagination,
    Sorting? Sorters,
    GetBudgetPermissionRequestsRequest? Payload) : IPagedQuery<IPagedList<GetBudgetPermissionRequestsResponse>>;