using System.Reflection;

using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest.DTO.Response;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Response;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Response;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.CancelAssigningParticipant;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.ConfirmAssigningParticipant;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.ExpireAssignParticipant;
using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermission.DTO.Response;
using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.CreateBudgetPermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.DeleteBudgetPermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.RemovePermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.RestoreBudgetPermission;
using Expenso.BudgetSharing.Application.Shared.QueryStore;
using Expenso.BudgetSharing.Domain;
using Expenso.BudgetSharing.Infrastructure;
using Expenso.BudgetSharing.Proxy;
using Expenso.BudgetSharing.Proxy.DTO.API.CreateBudgetPermission.Request;
using Expenso.BudgetSharing.Proxy.DTO.API.CreateBudgetPermission.Response;
using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Request;
using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Response;
using Expenso.Shared.Commands;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Modules;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Extensions = Expenso.BudgetSharing.Infrastructure.Extensions;

namespace Expenso.BudgetSharing.Api;

public sealed class BudgetSharingModule : ModuleDefinition
{
    public override string ModulePrefix => "/budget-sharing";

    public override IReadOnlyCollection<Assembly> GetModuleAssemblies()
    {
        return
        [
            typeof(BudgetSharingModule).Assembly,
            typeof(IBudgetPermissionQueryStore).Assembly,
            typeof(Extensions).Assembly,
            typeof(Domain.Extensions).Assembly,
            typeof(IBudgetSharingProxy).Assembly
        ];
    }

    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDomain();
        services.AddInfrastructure(configuration, ModuleName);
        services.AddBudgetSharingProxy(GetModuleAssemblies());
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
                [FromQuery] GetBudgetPermissionRequestsRequest_Status? status = null,
                [FromQuery] GetBudgetPermissionRequestsRequest_PermissionType? permissionType = null,
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

        EndpointRegistration expireAssigningParticipantEndpointRegistration = new(
            "budget-permission-requests/{id}/expire", "ExpireAssigningParticipant", AccessControl.User, HttpVerb.Patch,
            async ([FromServices] ICommandHandler<ExpireAssigningParticipantCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(new ExpireAssigningParticipantCommand(messageContextFactory.Current(), id),
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
            expireAssigningParticipantEndpointRegistration, cancelAssigningParticipantEndpointRegistration
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
                [FromQuery] GetBudgetPermissionsRequest_PermissionType? permissionType = null,
                CancellationToken cancellationToken = default) =>
            {
                IReadOnlyCollection<GetBudgetPermissionsResponse>? getPreferences = await handler.HandleAsync(
                    new GetBudgetPermissionsQuery(messageContextFactory.Current(), budgetId, ownerId, participantId,
                        PermissionType: permissionType, ForCurrentUser: forCurrentUser), cancellationToken);

                return Results.Ok(getPreferences);
            });

        EndpointRegistration createBudgetPermissionEndpointRegistration = new("budget-permissions",
            "CreateBudgetPermission", AccessControl.User, HttpVerb.Post, async (
                [FromServices] ICommandHandler<CreateBudgetPermissionCommand, CreateBudgetPermissionResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory,
                [FromBody] CreateBudgetPermissionRequest createBudgetPermissionRequest,
                CancellationToken cancellationToken = default) =>
            {
                CreateBudgetPermissionResponse? response = await handler.HandleAsync(
                    new CreateBudgetPermissionCommand(messageContextFactory.Current(), createBudgetPermissionRequest),
                    cancellationToken);

                return Results.CreatedAtRoute(getBudgetPermissionEndpointRegistration.Name, new
                {
                    id = response?.BudgetPermissionId
                }, response);
            });

        EndpointRegistration restoreBudgetPermissionEndpointRegistration = new("budget-permissions/{id}",
            "RestoreBudgetPermission", AccessControl.User, HttpVerb.Patch, async (
                [FromServices] ICommandHandler<RestoreBudgetPermissionCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(new RestoreBudgetPermissionCommand(messageContextFactory.Current(), id),
                    cancellationToken);

                return Results.NoContent();
            });

        EndpointRegistration deleteBudgetPermissionEndpointRegistration = new("budget-permissions/{id}",
            "DeleteBudgetPermission", AccessControl.User, HttpVerb.Delete, async (
                [FromServices] ICommandHandler<DeleteBudgetPermissionCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(new DeleteBudgetPermissionCommand(messageContextFactory.Current(), id),
                    cancellationToken);

                return Results.NoContent();
            });

        EndpointRegistration addPermissionEndpointRegistration = new(
            "budget-permissions/{id}/participants/{participantId}", "AddPermission", AccessControl.User, HttpVerb.Post,
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
            "budget-permissions/{id}/participants/{participantId}", "RemovePermission", AccessControl.User,
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
            getBudgetPermissionEndpointRegistration, getBudgetPermissionsEndpointRegistration,
            createBudgetPermissionEndpointRegistration, restoreBudgetPermissionEndpointRegistration,
            deleteBudgetPermissionEndpointRegistration, addPermissionEndpointRegistration,
            removePermissionEndpointRegistration
        ];
    }
}