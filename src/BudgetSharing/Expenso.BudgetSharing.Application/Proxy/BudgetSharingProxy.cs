using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions;
using Expenso.BudgetSharing.Shared;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Response;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.Shared.System.Modules.Constants;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.System.Types.Ordering;
using Expenso.Shared.System.Types.Pagination;

namespace Expenso.BudgetSharing.Application.Proxy;

internal sealed class BudgetSharingProxy : IBudgetSharingProxy
{
    private readonly IMessageContextFactory _messageContextFactory;
    private readonly IQueryDispatcher _queryDispatcher;

    public BudgetSharingProxy(IQueryDispatcher queryDispatcher, IMessageContextFactory messageContextFactory)
    {
        _messageContextFactory = messageContextFactory ??
                                 throw new ArgumentNullException(paramName: nameof(messageContextFactory));

        _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(paramName: nameof(queryDispatcher));
    }

    public async Task<IPagedList<GetBudgetPermissionsResponse>?> GetBudgetPermissionsAsync(
        GetBudgetPermissionsRequest request, Paging? pagination = null, Sorting? sorting = null,
        IMessageContext? messageContext = null, CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.QueryAsync(
            query: new GetBudgetPermissionsQuery(
                MessageContext: _messageContextFactory.FromParent(parent: messageContext,
                    moduleId: ModuleNames.BudgetSharingModule), Pagination: pagination ?? Paging.Default,
                Sorters: sorting ?? Sorting.Default, Payload: request), cancellationToken: cancellationToken);
    }
}