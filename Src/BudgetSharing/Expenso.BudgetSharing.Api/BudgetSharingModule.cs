using System.Reflection;

using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest.DTO.Responses;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Requests;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Responses;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Requests;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Responses;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.CancelAssigningParticipant;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.ConfirmAssigningParticipant;
using Expenso.BudgetSharing.Application.BudgetPermissions.Read.Internal.GetBudgetPermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Read.Internal.GetBudgetPermission.DTO.Responses;
using Expenso.BudgetSharing.Application.BudgetPermissions.Read.Internal.GetBudgetPermissions;
using Expenso.BudgetSharing.Application.BudgetPermissions.Read.Internal.GetBudgetPermissions.DTO.Requests;
using Expenso.BudgetSharing.Application.BudgetPermissions.Read.Internal.GetBudgetPermissions.DTO.Responses;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.Internal.AddPermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.Internal.AddPermission.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.Internal.CreateBudgetPermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.Internal.CreateBudgetPermission.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.Internal.RemovePermission;
using Expenso.BudgetSharing.Application.Shared.QueryStore;
using Expenso.BudgetSharing.Domain;
using Expenso.BudgetSharing.Infrastructure;
using Expenso.BudgetSharing.Proxy;
using Expenso.Shared.Commands;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Modules;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using DomainServiceCollectionExtensions = Expenso.BudgetSharing.Domain.ServiceCollectionExtensions;
using InfrastructureServiceCollectionExtensions = Expenso.BudgetSharing.Infrastructure.ServiceCollectionExtensions;

namespace Expenso.BudgetSharing.Api;

public sealed class BudgetSharingModule : ModuleDefinition
{
    public override string ModulePrefix => "/budget-sharing";

    public override Assembly[] GetModuleAssemblies()
    {
        return
        [
            typeof(BudgetSharingModule).Assembly,
            typeof(IBudgetPermissionQueryStore).Assembly,
            typeof(InfrastructureServiceCollectionExtensions).Assembly,
            typeof(DomainServiceCollectionExtensions).Assembly,
            typeof(IBudgetSharingProxy).Assembly
        ];
    }

    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDomain();
        services.AddInfrastructure(configuration, ModuleName);
    }

    public override IReadOnlyCollection<EndpointRegistration> CreateEndpoints()
    {
        List<EndpointRegistration> endpointsRegistration = [];
        endpointsRegistration.AddRange(CreateBudgetPermissionRequestEndpoints());
        endpointsRegistration.AddRange(CreateBudgetPermissionEndpoints());

        return endpointsRegistration;
    }

    private static IEnumerable<EndpointRegistration> CreateBudgetPermissionRequestEndpoints()
    {
        EndpointRegistration getBudgetPermissionRequestEndpointRegistration = new("budget-permission-requests/{id}",
            "GetBudgetPermissionRequest", AccessControl.User, HttpVerb.Get, async (
                [FromServices]
                IQueryHandler<GetBudgetPermissionRequestQuery, GetBudgetPermissionRequestResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                GetBudgetPermissionRequestResponse? getPreferences =
                    await handler.HandleAsync(new GetBudgetPermissionRequestQuery(messageContextFactory.Current(), id),
                        cancellationToken);

                return Results.Ok(getPreferences);
            });

        EndpointRegistration getBudgetPermissionRequestsEndpointRegistration = new("budget-permission-requests",
            "GetBudgetPermissionRequests", AccessControl.User, HttpVerb.Get, async (
                [FromServices]
                IQueryHandler<GetBudgetPermissionRequestsQuery,
                    IReadOnlyCollection<GetBudgetPermissionRequestsResponse>> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromQuery] Guid? budgetId = null,
                [FromQuery] Guid? participantId = null, [FromQuery] bool? forCurrentUser = null,
                [FromQuery] GetBudgetPermissionRequestsRequestStatus? status = null,
                [FromQuery] GetBudgetPermissionRequestsRequestPermissionType? permissionType = null,
                CancellationToken cancellationToken = default) =>
            {
                IReadOnlyCollection<GetBudgetPermissionRequestsResponse>? getPreferences = await handler.HandleAsync(
                    new GetBudgetPermissionRequestsQuery(messageContextFactory.Current(), budgetId, participantId,
                        forCurrentUser, status, permissionType), cancellationToken);

                return Results.Ok(getPreferences);
            });

        EndpointRegistration assignParticipantEndpointRegistration = new("budget-permission-requests",
            "AssignParticipant", AccessControl.User, HttpVerb.Post, async (
                [FromServices] ICommandHandler<AssignParticipantCommand, AssignParticipantResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory,
                [FromBody] AssignParticipantRequest assignParticipantRequest,
                CancellationToken cancellationToken = default) =>
            {
                AssignParticipantResponse? response = await handler.HandleAsync(
                    new AssignParticipantCommand(messageContextFactory.Current(), assignParticipantRequest),
                    cancellationToken);

                return Results.CreatedAtRoute(getBudgetPermissionRequestEndpointRegistration.Name, new
                {
                    id = response?.BudgetPermissionRequestId
                }, response);
            });

        EndpointRegistration confirmAssigningParticipantEndpointRegistration = new(
            "budget-permission-requests/{id}/confirm", "ConfirmAssigningParticipant", AccessControl.User,
            HttpVerb.Patch, async ([FromServices] ICommandHandler<ConfirmAssigningParticipantCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(new ConfirmAssigningParticipantCommand(messageContextFactory.Current(), id),
                    cancellationToken);

                return Results.NoContent();
            });

        EndpointRegistration cancelAssigningParticipantEndpointRegistration = new(
            "budget-permission-requests/{id}/cancel", "CancelAssigningParticipant", AccessControl.User, HttpVerb.Patch,
            async ([FromServices] ICommandHandler<CancelAssigningParticipantCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(new CancelAssigningParticipantCommand(messageContextFactory.Current(), id),
                    cancellationToken);

                return Results.NoContent();
            });

        return
        [
            getBudgetPermissionRequestEndpointRegistration, getBudgetPermissionRequestsEndpointRegistration,
            assignParticipantEndpointRegistration, confirmAssigningParticipantEndpointRegistration,
            cancelAssigningParticipantEndpointRegistration
        ];
    }

    private static IEnumerable<EndpointRegistration> CreateBudgetPermissionEndpoints()
    {
        EndpointRegistration getBudgetPermissionEndpointRegistration = new("budget-permissions/{id}",
            "GetBudgetPermission", AccessControl.User, HttpVerb.Get, async (
                [FromServices] IQueryHandler<GetBudgetPermissionQuery, GetBudgetPermissionResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                GetBudgetPermissionResponse? getPreferences = await handler.HandleAsync(
                    new GetBudgetPermissionQuery(messageContextFactory.Current(), id), cancellationToken);

                return Results.Ok(getPreferences);
            });

        EndpointRegistration getBudgetPermissionsEndpointRegistration = new("budget-permissions",
            "GetBudgetPermissions", AccessControl.User, HttpVerb.Get, async (
                [FromServices]
                IQueryHandler<GetBudgetPermissionsQuery, IReadOnlyCollection<GetBudgetPermissionsResponse>> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromQuery] Guid? budgetId = null,
                [FromQuery] Guid? ownerId = null, [FromQuery] Guid? participantId = null,
                [FromQuery] bool? forCurrentUser = null,
                [FromQuery] GetBudgetPermissionsRequestPermissionType? permissionType = null,
                CancellationToken cancellationToken = default) =>
            {
                IReadOnlyCollection<GetBudgetPermissionsResponse>? getPreferences = await handler.HandleAsync(
                    new GetBudgetPermissionsQuery(messageContextFactory.Current(), budgetId, ownerId, participantId,
                        PermissionType: permissionType, ForCurrentUser: forCurrentUser), cancellationToken);

                return Results.Ok(getPreferences);
            });

        EndpointRegistration createBudgetPermission = new("budget-permissions", "CreateBudgetPermission",
            AccessControl.User, HttpVerb.Post, async (
                [FromServices] ICommandHandler<CreateBudgetPermissionCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory,
                [FromBody] CreateBudgetPermissionRequest createBudgetPermissionRequest,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    new CreateBudgetPermissionCommand(messageContextFactory.Current(), createBudgetPermissionRequest),
                    cancellationToken);

                return Results.NoContent();
            });

        EndpointRegistration addPermissionEndpointRegistration = new(
            "budget-permissions/{id}/permissions/{participantId}", "AddPermission", AccessControl.User, HttpVerb.Post,
            async ([FromServices] ICommandHandler<AddPermissionCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                [FromRoute] Guid participantId, [FromBody] AddPermissionRequest addPermissionRequest,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    new AddPermissionCommand(messageContextFactory.Current(), id, participantId, addPermissionRequest),
                    cancellationToken);

                return Results.NoContent();
            });

        EndpointRegistration removePermissionEndpointRegistration = new(
            "budget-permissions/{id}/permissions/{participantId}", "RemovePermission", AccessControl.User,
            HttpVerb.Delete, async ([FromServices] ICommandHandler<RemovePermissionCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                [FromRoute] Guid participantId, CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    new RemovePermissionCommand(messageContextFactory.Current(), id, participantId), cancellationToken);

                return Results.NoContent();
            });

        return
        [
            getBudgetPermissionEndpointRegistration, getBudgetPermissionsEndpointRegistration, createBudgetPermission,
            addPermissionEndpointRegistration, removePermissionEndpointRegistration
        ];
    }
}