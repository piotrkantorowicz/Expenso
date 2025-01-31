using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant;
using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.CreateBudgetPermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.DeleteBudgetPermission;
using Expenso.BudgetSharing.Shared;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissionRequests.AssignParticipant.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissionRequests.AssignParticipant.Response;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.AddPermission.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.CreateBudgetPermission.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.CreateBudgetPermission.Response;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.DeleteBudgetPermission.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Response;
using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.Shared.System.Modules.Constants;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.System.Types.Ordering;
using Expenso.Shared.System.Types.Paging;

namespace Expenso.BudgetSharing.Application.Proxy;

internal sealed class BudgetSharingProxy : IBudgetSharingProxy
{
    private readonly IMessageContextFactory _messageContextFactory;
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public BudgetSharingProxy(IQueryDispatcher queryDispatcher, IMessageContextFactory messageContextFactory,
        ICommandDispatcher commandDispatcher)
    {
        _messageContextFactory = messageContextFactory ??
                                 throw new ArgumentNullException(paramName: nameof(messageContextFactory));

        _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(paramName: nameof(commandDispatcher));
        _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(paramName: nameof(queryDispatcher));
    }

    public async Task<IPagedList<GetBudgetPermissionsResponse>?> GetBudgetPermissionsAsync(
        GetBudgetPermissionsRequest request, Pagination? pagination = null, Sorting? sorting = null,
        IMessageContext? messageContext = null, CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.QueryAsync(
            query: new GetBudgetPermissionsQuery(
                MessageContext: _messageContextFactory.FromParent(parent: messageContext,
                    moduleId: ModuleNames.BudgetSharingModule), Pagination: pagination, Sorters: sorting,
                Payload: request), cancellationToken: cancellationToken);
    }

    public async Task<AssignParticipantResponse?> AssignParticipantAsync(AssignParticipantRequest request,
        IMessageContext? messageContext = null, CancellationToken cancellationToken = default)
    {
        return await _commandDispatcher.SendAsync<AssignParticipantCommand, AssignParticipantResponse>(
            command: new AssignParticipantCommand(
                MessageContext: _messageContextFactory.FromParent(parent: messageContext,
                    moduleId: ModuleNames.BudgetSharingModule), Payload: request),
            cancellationToken: cancellationToken);
    }

    public async Task<CreateBudgetPermissionResponse?> CreateBudgetPermissionAsync(
        CreateBudgetPermissionRequest request, IMessageContext? messageContext = null,
        CancellationToken cancellationToken = default)
    {
        return await _commandDispatcher.SendAsync<CreateBudgetPermissionCommand, CreateBudgetPermissionResponse>(
            command: new CreateBudgetPermissionCommand(
                MessageContext: _messageContextFactory.FromParent(parent: messageContext,
                    moduleId: ModuleNames.BudgetSharingModule), Payload: request),
            cancellationToken: cancellationToken);
    }

    public async Task AddPermissionAsync(AddPermissionRequest request, IMessageContext? messageContext = null,
        CancellationToken cancellationToken = default)
    {
        await _commandDispatcher.SendAsync(
            command: new AddPermissionCommand(
                MessageContext: _messageContextFactory.FromParent(parent: messageContext,
                    moduleId: ModuleNames.BudgetSharingModule), Payload: request),
            cancellationToken: cancellationToken);
    }

    public async Task DeleteBudgetPermissionAsync(DeleteBudgetPermissionRequest request,
        IMessageContext? messageContext = null, CancellationToken cancellationToken = default)
    {
        await _commandDispatcher.SendAsync(
            command: new DeleteBudgetPermissionCommand(
                MessageContext: _messageContextFactory.FromParent(parent: messageContext,
                    moduleId: ModuleNames.BudgetSharingModule), Payload: request),
            cancellationToken: cancellationToken);
    }
}