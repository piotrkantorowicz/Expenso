using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Response;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.System.Types.Ordering;
using Expenso.Shared.System.Types.Pagination;

namespace Expenso.BudgetSharing.Shared;

public interface IBudgetSharingProxy
{
    Task<IPagedList<GetBudgetPermissionsResponse>?> GetBudgetPermissionsAsync(GetBudgetPermissionsRequest request,
        Paging? pagination = null, Sorting? sorting = null, IMessageContext? messageContext = null,
        CancellationToken cancellationToken = default);
}