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

    public async Task<IReadOnlyCollection<GetBudgetPermissionsResponse>?> GetBudgetPermissionsAsync(Guid budgetId,
        CancellationToken cancellationToken = default)
    {
        GetBudgetPermissionsQuery query = new(MessageContext: _messageContextFactory.Current(), BudgetId: budgetId);

        IReadOnlyCollection<GetBudgetPermissionsResponse>? getBudgetPermissionsResponse =
            await _queryDispatcher.QueryAsync(query: query, cancellationToken: cancellationToken);

        return GetBudgetPermissionsExternalResponseMap.MapTo(budgetPermissions: getBudgetPermissionsResponse);
    }

    public async Task<CreateBudgetPermissionResponse?> CreateBudgetPermission(
        CreateBudgetPermissionRequest createBudgetPermissionRequest, CancellationToken cancellationToken = default)
    {
        CreateBudgetPermissionCommand command = new(MessageContext: _messageContextFactory.Current(),
            CreateBudgetPermissionRequest: createBudgetPermissionRequest);

        CreateBudgetPermissionResponse? createBudgetPermissionResponse =
            await _commandDispatcher.SendAsync<CreateBudgetPermissionCommand, CreateBudgetPermissionResponse>(
                command: command, cancellationToken: cancellationToken);

        return createBudgetPermissionResponse is null
            ? null
            : new CreateBudgetPermissionResponse(BudgetPermissionId: createBudgetPermissionResponse.BudgetPermissionId);
    }

    public async Task DeleteBudgetPermission(Guid budgetPermissionId, CancellationToken cancellationToken = default)
    {
        DeleteBudgetPermissionCommand command = new(MessageContext: _messageContextFactory.Current(),
            BudgetPermissionId: budgetPermissionId);

        await _commandDispatcher.SendAsync(command: command, cancellationToken: cancellationToken);
    }

    public async Task RestoreBudgetPermission(Guid budgetPermissionId, CancellationToken cancellationToken = default)
    {
        RestoreBudgetPermissionCommand command = new(MessageContext: _messageContextFactory.Current(),
            BudgetPermissionId: budgetPermissionId);

        await _commandDispatcher.SendAsync(command: command, cancellationToken: cancellationToken);
    }
}