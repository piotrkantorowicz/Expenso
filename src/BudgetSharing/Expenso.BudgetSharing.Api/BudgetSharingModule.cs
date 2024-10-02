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

public sealed class BudgetSharingModule : IModuleDefinition
{
    public string ModulePrefix => "/budget-sharing";

    public IReadOnlyCollection<Assembly> GetModuleAssemblies()
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

    public void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDomain();
        services.AddInfrastructure(configuration: configuration, moduleName: GetType().Name);
        services.AddBudgetSharingProxy(assemblies: GetModuleAssemblies());
    }

    public IReadOnlyCollection<EndpointRegistration> CreateEndpoints()
    {
        List<EndpointRegistration> endpointsRegistration = [];
        endpointsRegistration.AddRange(collection: CreateBudgetPermissionRequestEndpoints());
        endpointsRegistration.AddRange(collection: CreateBudgetPermissionEndpoints());

        return endpointsRegistration;
    }

    private static IEnumerable<EndpointRegistration> CreateBudgetPermissionRequestEndpoints()
    {
        EndpointRegistration getBudgetPermissionRequestEndpointRegistration = new(
            Pattern: "budget-permission-requests/{id}", Name: "GetBudgetPermissionRequest",
            AccessControl: AccessControl.User, HttpVerb: HttpVerb.Get, Handler: async (
                [FromServices]
                IQueryHandler<GetBudgetPermissionRequestQuery, GetBudgetPermissionRequestResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                GetBudgetPermissionRequestResponse? getPreferences =
                    await handler.HandleAsync(
                        query: new GetBudgetPermissionRequestQuery(MessageContext: messageContextFactory.Current(),
                            BudgetPermissionRequestId: id), cancellationToken: cancellationToken);

                return Results.Ok(value: getPreferences);
            });

        EndpointRegistration getBudgetPermissionRequestsEndpointRegistration = new(
            Pattern: "budget-permission-requests", Name: "GetBudgetPermissionRequests",
            AccessControl: AccessControl.User, HttpVerb: HttpVerb.Get, Handler: async (
                [FromServices]
                IQueryHandler<GetBudgetPermissionRequestsQuery,
                    IReadOnlyCollection<GetBudgetPermissionRequestsResponse>> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromQuery] Guid? budgetId = null,
                [FromQuery] Guid? participantId = null, [FromQuery] Guid? ownerId = null,
                [FromQuery] bool? forCurrentUser = null,
                [FromQuery] GetBudgetPermissionRequestsRequest_Status? status = null,
                [FromQuery] GetBudgetPermissionRequestsRequest_PermissionType? permissionType = null,
                CancellationToken cancellationToken = default) =>
            {
                IReadOnlyCollection<GetBudgetPermissionRequestsResponse>? getPreferences = await handler.HandleAsync(
                    query: new GetBudgetPermissionRequestsQuery(MessageContext: messageContextFactory.Current(),
                        BudgetId: budgetId, ParticipantId: participantId, OwnerId: ownerId,
                        ForCurrentUser: forCurrentUser, Status: status, PermissionType: permissionType),
                    cancellationToken: cancellationToken);

                return Results.Ok(value: getPreferences);
            });

        EndpointRegistration assignParticipantEndpointRegistration = new(Pattern: "budget-permission-requests",
            Name: "AssignParticipant", AccessControl: AccessControl.User, HttpVerb: HttpVerb.Post, Handler: async (
                [FromServices] ICommandHandler<AssignParticipantCommand, AssignParticipantResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory,
                [FromBody] AssignParticipantRequest assignParticipantRequest,
                CancellationToken cancellationToken = default) =>
            {
                AssignParticipantResponse? response = await handler.HandleAsync(
                    command: new AssignParticipantCommand(MessageContext: messageContextFactory.Current(),
                        AssignParticipantRequest: assignParticipantRequest), cancellationToken: cancellationToken);

                return Results.CreatedAtRoute(routeName: getBudgetPermissionRequestEndpointRegistration.Name,
                    routeValues: new
                    {
                        id = response?.BudgetPermissionRequestId
                    }, value: response);
            });

        EndpointRegistration confirmAssigningParticipantEndpointRegistration = new(
            Pattern: "budget-permission-requests/{id}/confirm", Name: "ConfirmAssigningParticipant",
            AccessControl: AccessControl.User, HttpVerb: HttpVerb.Patch, Handler: async (
                [FromServices] ICommandHandler<ConfirmAssigningParticipantCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    command: new ConfirmAssigningParticipantCommand(MessageContext: messageContextFactory.Current(),
                        BudgetPermissionRequestId: id), cancellationToken: cancellationToken);

                return Results.NoContent();
            });

        EndpointRegistration expireAssigningParticipantEndpointRegistration = new(
            Pattern: "budget-permission-requests/{id}/expire", Name: "ExpireAssigningParticipant",
            AccessControl: AccessControl.User, HttpVerb: HttpVerb.Patch, Handler: async (
                [FromServices] ICommandHandler<ExpireAssigningParticipantCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    command: new ExpireAssigningParticipantCommand(MessageContext: messageContextFactory.Current(),
                        BudgetPermissionRequestId: id), cancellationToken: cancellationToken);

                return Results.NoContent();
            });

        EndpointRegistration cancelAssigningParticipantEndpointRegistration = new(
            Pattern: "budget-permission-requests/{id}/cancel", Name: "CancelAssigningParticipant",
            AccessControl: AccessControl.User, HttpVerb: HttpVerb.Patch, Handler: async (
                [FromServices] ICommandHandler<CancelAssigningParticipantCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    command: new CancelAssigningParticipantCommand(MessageContext: messageContextFactory.Current(),
                        BudgetPermissionRequestId: id), cancellationToken: cancellationToken);

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
        EndpointRegistration getBudgetPermissionEndpointRegistration = new(Pattern: "budget-permissions/{id}",
            Name: "GetBudgetPermission", AccessControl: AccessControl.User, HttpVerb: HttpVerb.Get, Handler: async (
                [FromServices] IQueryHandler<GetBudgetPermissionQuery, GetBudgetPermissionResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                GetBudgetPermissionResponse? getPreferences = await handler.HandleAsync(
                    query: new GetBudgetPermissionQuery(MessageContext: messageContextFactory.Current(),
                        BudgetPermissionId: id), cancellationToken: cancellationToken);

                return Results.Ok(value: getPreferences);
            });

        EndpointRegistration getBudgetPermissionsEndpointRegistration = new(Pattern: "budget-permissions",
            Name: "GetBudgetPermissions", AccessControl: AccessControl.User, HttpVerb: HttpVerb.Get, Handler: async (
                [FromServices]
                IQueryHandler<GetBudgetPermissionsQuery, IReadOnlyCollection<GetBudgetPermissionsResponse>> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromQuery] Guid? budgetId = null,
                [FromQuery] Guid? ownerId = null, [FromQuery] Guid? participantId = null,
                [FromQuery] bool? forCurrentUser = null,
                [FromQuery] GetBudgetPermissionsRequest_PermissionType? permissionType = null,
                CancellationToken cancellationToken = default) =>
            {
                IReadOnlyCollection<GetBudgetPermissionsResponse>? getPreferences = await handler.HandleAsync(
                    query: new GetBudgetPermissionsQuery(MessageContext: messageContextFactory.Current(),
                        BudgetId: budgetId, OwnerId: ownerId, ParticipantId: participantId,
                        PermissionType: permissionType, ForCurrentUser: forCurrentUser),
                    cancellationToken: cancellationToken);

                return Results.Ok(value: getPreferences);
            });

        EndpointRegistration createBudgetPermissionEndpointRegistration = new(Pattern: "budget-permissions",
            Name: "CreateBudgetPermission", AccessControl: AccessControl.User, HttpVerb: HttpVerb.Post, Handler: async (
                [FromServices] ICommandHandler<CreateBudgetPermissionCommand, CreateBudgetPermissionResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory,
                [FromBody] CreateBudgetPermissionRequest createBudgetPermissionRequest,
                CancellationToken cancellationToken = default) =>
            {
                CreateBudgetPermissionResponse? response = await handler.HandleAsync(
                    command: new CreateBudgetPermissionCommand(MessageContext: messageContextFactory.Current(),
                        CreateBudgetPermissionRequest: createBudgetPermissionRequest),
                    cancellationToken: cancellationToken);

                return Results.CreatedAtRoute(routeName: getBudgetPermissionEndpointRegistration.Name, routeValues: new
                {
                    id = response?.BudgetPermissionId
                }, value: response);
            });

        EndpointRegistration restoreBudgetPermissionEndpointRegistration = new(Pattern: "budget-permissions/{id}",
            Name: "RestoreBudgetPermission", AccessControl: AccessControl.User, HttpVerb: HttpVerb.Patch,
            Handler: async ([FromServices] ICommandHandler<RestoreBudgetPermissionCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    command: new RestoreBudgetPermissionCommand(MessageContext: messageContextFactory.Current(),
                        BudgetPermissionId: id), cancellationToken: cancellationToken);

                return Results.NoContent();
            });

        EndpointRegistration deleteBudgetPermissionEndpointRegistration = new(Pattern: "budget-permissions/{id}",
            Name: "DeleteBudgetPermission", AccessControl: AccessControl.User, HttpVerb: HttpVerb.Delete,
            Handler: async ([FromServices] ICommandHandler<DeleteBudgetPermissionCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    command: new DeleteBudgetPermissionCommand(MessageContext: messageContextFactory.Current(),
                        BudgetPermissionId: id), cancellationToken: cancellationToken);

                return Results.NoContent();
            });

        EndpointRegistration addPermissionEndpointRegistration = new(
            Pattern: "budget-permissions/{id}/participants/{participantId}", Name: "AddPermission",
            AccessControl: AccessControl.User, HttpVerb: HttpVerb.Post, Handler: async (
                [FromServices] ICommandHandler<AddPermissionCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                [FromRoute] Guid participantId, [FromBody] AddPermissionRequest addPermissionRequest,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    command: new AddPermissionCommand(MessageContext: messageContextFactory.Current(),
                        BudgetPermissionId: id, ParticipantId: participantId,
                        AddPermissionRequest: addPermissionRequest), cancellationToken: cancellationToken);

                return Results.NoContent();
            });

        EndpointRegistration removePermissionEndpointRegistration = new(
            Pattern: "budget-permissions/{id}/participants/{participantId}", Name: "RemovePermission",
            AccessControl: AccessControl.User, HttpVerb: HttpVerb.Delete, Handler: async (
                [FromServices] ICommandHandler<RemovePermissionCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                [FromRoute] Guid participantId, CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    command: new RemovePermissionCommand(MessageContext: messageContextFactory.Current(),
                        BudgetPermissionId: id, ParticipantId: participantId), cancellationToken: cancellationToken);

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