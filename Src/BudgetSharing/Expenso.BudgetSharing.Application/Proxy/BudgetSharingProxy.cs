using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions;
using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions.DTO.Request.Maps;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.CreateBudgetPermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.DeleteBudgetPermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.RestoreBudgetPermission;
using Expenso.BudgetSharing.Proxy;
using Expenso.BudgetSharing.Proxy.DTO.API.CreateBudgetPermission.Request;
using Expenso.BudgetSharing.Proxy.DTO.API.CreateBudgetPermission.Response;
using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Response;
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
        GetBudgetPermissionsQuery query = new(_messageContextFactory.Current(), budgetId);

        IReadOnlyCollection<GetBudgetPermissionsResponse>? getBudgetPermissionsResponse =
            await _queryDispatcher.QueryAsync(query, cancellationToken);

        return GetBudgetPermissionsExternalResponseMap.MapTo(getBudgetPermissionsResponse);
    }

    public async Task<CreateBudgetPermissionResponse?> CreateBudgetPermission(
        CreateBudgetPermissionRequest createBudgetPermissionRequest, CancellationToken cancellationToken = default)
    {
        CreateBudgetPermissionCommand command = new(_messageContextFactory.Current(), createBudgetPermissionRequest);

        CreateBudgetPermissionResponse? createBudgetPermissionResponse =
            await _commandDispatcher.SendAsync<CreateBudgetPermissionCommand, CreateBudgetPermissionResponse>(command,
                cancellationToken);

        return createBudgetPermissionResponse is null
            ? null
            : new CreateBudgetPermissionResponse(createBudgetPermissionResponse.BudgetPermissionId);
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