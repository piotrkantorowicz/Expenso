using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Request;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.System.Types.Pagination;

namespace Expenso.BudgetSharing.Shared;

public interface IBudgetSharingProxy
{
    Task<IPagedList?> GetBudgetPermissionsAsync(GetBudgetPermissionsRequest request, Paging? pagination = null,
        IMessageContext? messageContext = null,
        CancellationToken cancellationToken = default);
}