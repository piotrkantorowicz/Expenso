using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Response;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.System.Types.Ordering;
using Expenso.Shared.System.Types.Paging;

namespace Expenso.BudgetSharing.Shared;

public interface IBudgetSharingProxy
{
    Task<IPagedList<GetBudgetPermissionsResponse>?> GetBudgetPermissionsAsync(GetBudgetPermissionsRequest request,
        Pagination? pagination = null, Sorting? sorting = null, IMessageContext? messageContext = null,
        CancellationToken cancellationToken = default);
}