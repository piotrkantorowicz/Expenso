using Expenso.BudgetSharing.Proxy;
using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.Queries.Dispatchers;

namespace Expenso.BudgetSharing.Application.Proxy;

internal sealed class BudgetSharingProxy : IBudgetSharingProxy
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public BudgetSharingProxy(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
        _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));
    }
}