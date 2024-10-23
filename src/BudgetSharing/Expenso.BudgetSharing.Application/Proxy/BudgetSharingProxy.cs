using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions;
using Expenso.BudgetSharing.Shared;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Response;
using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.Proxy;

internal sealed class BudgetSharingProxy : IBudgetSharingProxy
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IMessageContextFactory _messageContextFactory;
    private readonly IQueryDispatcher _queryDispatcher;

    public BudgetSharingProxy(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher,
        IMessageContextFactory messageContextFactory)
    {
        _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(paramName: nameof(commandDispatcher));

        _messageContextFactory = messageContextFactory ??
                                 throw new ArgumentNullException(paramName: nameof(messageContextFactory));

        _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(paramName: nameof(queryDispatcher));
    }

    public async Task<IReadOnlyCollection<GetBudgetPermissionsResponse>?> GetBudgetPermissionsAsync(
        GetBudgetPermissionsRequest request, CancellationToken cancellationToken = default)
    {
        GetBudgetPermissionsQuery query = new(MessageContext: _messageContextFactory.Current(), Payload: request);

        return await _queryDispatcher.QueryAsync(query: query, cancellationToken: cancellationToken);
    }
}