using Expenso.BudgetSharing.Application.BudgetPermissions.Read.External.GetBudgetPermissions;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.External.CreateBudgetPermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.External.RemoveBudgetPermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.External.RestoreBudgetPermission;
using Expenso.BudgetSharing.Proxy;
using Expenso.BudgetSharing.Proxy.DTO.API.CreateBudgetPermission.Requests;
using Expenso.BudgetSharing.Proxy.DTO.API.CreateBudgetPermission.Responses;
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

    public async Task<CreateBudgetPermissionResponse?> CreateBudgetPermission(Guid? budgetPermissionId, Guid budgetId,
        Guid ownerId, CancellationToken cancellationToken = default)
    {
        CreateBudgetPermissionCommand command = new(_messageContextFactory.Current(),
            new CreateBudgetPermissionRequest(budgetPermissionId, budgetId, ownerId));

        return await _commandDispatcher.SendAsync<CreateBudgetPermissionCommand, CreateBudgetPermissionResponse>(
            command, cancellationToken);
    }

    public async Task DeleteBudgetPermission(Guid budgetPermissionId, CancellationToken cancellationToken = default)
    {
        DeleteBudgetPermissionCommand command = new(_messageContextFactory.Current(), budgetPermissionId);
        await _commandDispatcher.SendAsync(command, cancellationToken);
    }

    public async Task RestoreBudgetPermission(Guid budgetPermissionId, CancellationToken cancellationToken = default)
    {
        RestoreBudgetPermissionCommand command = new(_messageContextFactory.Current(), budgetPermissionId);
        await _commandDispatcher.SendAsync(command, cancellationToken);
    }
}