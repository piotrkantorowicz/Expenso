using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Response;
using Expenso.Shared.Queries.Pagination;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.System.Types.Ordering;
using Expenso.Shared.System.Types.Paging;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions;

public sealed record GetBudgetPermissionsQuery(
    IMessageContext MessageContext,
    Pagination? Pagination,
    Sorting? Sorters,
    GetBudgetPermissionsRequest? Payload) : IPagedQuery<IPagedList<GetBudgetPermissionsResponse>>;