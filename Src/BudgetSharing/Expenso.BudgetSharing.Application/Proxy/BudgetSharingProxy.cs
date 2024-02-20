using Expenso.BudgetSharing.Application.Proxy.Read.GetBudgetPermissions;
using Expenso.BudgetSharing.Application.Proxy.Write.AssignOwnerPermission;
using Expenso.BudgetSharing.Proxy;
using Expenso.BudgetSharing.Proxy.DTO.API.AssignOwnerPermission.Requests;
using Expenso.BudgetSharing.Proxy.DTO.API.AssignOwnerPermission.Responses;
using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Responses;
using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.Proxy;

internal sealed class BudgetSharingProxy(
    ICommandDispatcher commandDispatcher,
    IQueryDispatcher queryDispatcher,
    IMessageContextFactory messageContextFactory) : IBudgetSharingProxy
{
    private readonly ICommandDispatcher _commandDispatcher =
        commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));

    private readonly IMessageContextFactory _messageContextFactory = messageContextFactory ??
                                                                     throw new ArgumentNullException(
                                                                         nameof(messageContextFactory));

    private readonly IQueryDispatcher _queryDispatcher =
        queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));

    public async Task<IReadOnlyCollection<GetBudgetPermissionsResponse>?> GetBudgetPermissionsAsync(Guid budgetId,
        CancellationToken cancellationToken = default)
    {
        GetBudgetPermissionsQuery query = new(_messageContextFactory.Current(), BudgetId: budgetId);

        IReadOnlyCollection<GetBudgetPermissionsResponse>? budgetPermissions =
            await _queryDispatcher.QueryAsync(query, cancellationToken);

        return budgetPermissions;
    }

    public async Task<AssignOwnerPermissionResponse?> AssignOwnerPermission(Guid budgetPermissionId, Guid budgetId,
        Guid ownerId, CancellationToken cancellationToken = default)
    {
        AssignOwnerPermissionCommand command = new(_messageContextFactory.Current(), budgetPermissionId,
            new AssignOwnerPermissionRequest(budgetId, ownerId));

        return await _commandDispatcher.SendAsync<AssignOwnerPermissionCommand, AssignOwnerPermissionResponse>(command,
            cancellationToken);
    }
}