using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Response;
using Expenso.Shared.Queries.Pagination;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.System.Types.Pagination;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests;

public sealed record GetBudgetPermissionRequestsQuery(
    IMessageContext MessageContext,
    Paging? Pagination,
    GetBudgetPermissionRequestsRequest? Payload) : IPagedQuery<IPagedList<GetBudgetPermissionRequestsResponse>>;