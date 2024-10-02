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
using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.CreateBudgetPermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.DeleteBudgetPermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.RemovePermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.RestoreBudgetPermission;
using Expenso.BudgetSharing.Application.Shared.QueryStore;
using Expenso.BudgetSharing.Domain;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
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
            pattern: "budget-permission-requests/{id}", name: "GetBudgetPermissionRequest",
            accessControl: AccessControl.User, httpVerb: HttpVerb.Get, subModule: nameof(BudgetPermissionRequest),
            handler: async (
                [FromServices]
                IQueryHandler<GetBudgetPermissionRequestQuery, GetBudgetPermissionRequestResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                GetBudgetPermissionRequestResponse? response = await handler.HandleAsync(
                    query: new GetBudgetPermissionRequestQuery(MessageContext: messageContextFactory.Current(),
                        BudgetPermissionRequestId: id), cancellationToken: cancellationToken);

                return Results.Ok(value: response);
            }, description: "Fetches the details of a specific budget permission request using its unique identifier.",
            summary: "Retrieve budget permission request by Id", responses:
            [
                new Produces(StatusCode: 200, ContentType: typeof(GetBudgetPermissionRequestResponse))
            ]);

        EndpointRegistration getBudgetPermissionRequestsEndpointRegistration = new(
            pattern: "budget-permission-requests", name: "GetBudgetPermissionRequests",
            accessControl: AccessControl.User, httpVerb: HttpVerb.Get, subModule: nameof(BudgetPermissionRequest),
            handler: async (
                [FromServices]
                IQueryHandler<GetBudgetPermissionRequestsQuery,
                    IReadOnlyCollection<GetBudgetPermissionRequestsResponse>> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromQuery] Guid? budgetId = null,
                [FromQuery] Guid? participantId = null, [FromQuery] bool? forCurrentUser = null,
                [FromQuery] GetBudgetPermissionRequestsRequest_Status? status = null,
                [FromQuery] GetBudgetPermissionRequestsRequest_PermissionType? permissionType = null,
                CancellationToken cancellationToken = default) =>
            {
                IReadOnlyCollection<GetBudgetPermissionRequestsResponse>? response = await handler.HandleAsync(
                    query: new GetBudgetPermissionRequestsQuery(MessageContext: messageContextFactory.Current(),
                        BudgetId: budgetId, ParticipantId: participantId, ForCurrentUser: forCurrentUser,
                        Status: status, PermissionType: permissionType), cancellationToken: cancellationToken);

                return Results.Ok(value: response);
            }, description: "Retrieves a list of budget permission requests filtered by optional parameters.",
            summary: "Retrieve multiple budget permission requests with optional filters", responses:
            [
                new Produces(StatusCode: 200,
                    ContentType: typeof(IReadOnlyCollection<GetBudgetPermissionRequestsResponse>))
            ]);

        EndpointRegistration assignParticipantEndpointRegistration = new(pattern: "budget-permission-requests",
            name: "AssignParticipant", accessControl: AccessControl.User, httpVerb: HttpVerb.Post,
            subModule: nameof(BudgetPermissionRequest), handler: async (
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
            }, description: "Assigns a participant to a budget permission request by providing the request details.",
            summary: "Assign participant to a budget permission request", responses:
            [
                new Produces(StatusCode: 201, ContentType: typeof(AssignParticipantResponse))
            ]);

        EndpointRegistration confirmAssigningParticipantEndpointRegistration = new(
            pattern: "budget-permission-requests/{id}/confirm", name: "ConfirmAssigningParticipant",
            accessControl: AccessControl.User, httpVerb: HttpVerb.Patch, subModule: nameof(BudgetPermissionRequest),
            handler: async ([FromServices] ICommandHandler<ConfirmAssigningParticipantCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    command: new ConfirmAssigningParticipantCommand(MessageContext: messageContextFactory.Current(),
                        BudgetPermissionRequestId: id), cancellationToken: cancellationToken);

                return Results.NoContent();
            }, description: "Confirms the assignment of a participant to a budget permission request.",
            summary: "Confirm assigning participant to a budget permission request", responses:
            [
                new Produces(StatusCode: 204)
            ]);

        EndpointRegistration expireAssigningParticipantEndpointRegistration = new(
            pattern: "budget-permission-requests/{id}/expire", name: "ExpireAssigningParticipant",
            accessControl: AccessControl.User, httpVerb: HttpVerb.Patch, subModule: nameof(BudgetPermissionRequest),
            handler: async ([FromServices] ICommandHandler<ExpireAssigningParticipantCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    command: new ExpireAssigningParticipantCommand(MessageContext: messageContextFactory.Current(),
                        BudgetPermissionRequestId: id), cancellationToken: cancellationToken);

                return Results.NoContent();
            }, description: "Expires the assignment of a participant to a budget permission request.",
            summary: "Expire assigning participant to a budget permission request", responses:
            [
                new Produces(StatusCode: 204)
            ]);

        EndpointRegistration cancelAssigningParticipantEndpointRegistration = new(
            pattern: "budget-permission-requests/{id}/cancel", name: "CancelAssigningParticipant",
            accessControl: AccessControl.User, httpVerb: HttpVerb.Patch, subModule: nameof(BudgetPermissionRequest),
            handler: async ([FromServices] ICommandHandler<CancelAssigningParticipantCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    command: new CancelAssigningParticipantCommand(MessageContext: messageContextFactory.Current(),
                        BudgetPermissionRequestId: id), cancellationToken: cancellationToken);

                return Results.NoContent();
            }, description: "Cancels the assignment of a participant to a budget permission request.",
            summary: "Cancel assigning participant to a budget permission request", responses:
            [
                new Produces(StatusCode: 204)
            ]);

        return
        [
            getBudgetPermissionRequestEndpointRegistration,
            getBudgetPermissionRequestsEndpointRegistration,
            assignParticipantEndpointRegistration,
            confirmAssigningParticipantEndpointRegistration,
            expireAssigningParticipantEndpointRegistration,
            cancelAssigningParticipantEndpointRegistration
        ];
    }

    private static IEnumerable<EndpointRegistration> CreateBudgetPermissionEndpoints()
    {
        EndpointRegistration getBudgetPermissionEndpointRegistration = new(pattern: "budget-permissions/{id}",
            name: "GetBudgetPermission", accessControl: AccessControl.User, httpVerb: HttpVerb.Get,
            subModule: nameof(BudgetPermission), handler: async (
                [FromServices]
                IQueryHandler<GetBudgetPermissionQuery, Application.BudgetPermissions.Read.GetBudgetPermission.DTO.
                    Response.GetBudgetPermissionResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                Application.BudgetPermissions.Read.GetBudgetPermission.DTO.Response.GetBudgetPermissionResponse?
                    getPreferences = await handler.HandleAsync(
                        query: new GetBudgetPermissionQuery(MessageContext: messageContextFactory.Current(),
                            BudgetPermissionId: id), cancellationToken: cancellationToken);

                return Results.Ok(value: getPreferences);
            }, description: "Fetches the details of a specific budget permission using its unique identifier.",
            summary: "Retrieve budget permission information by Id", responses:
            [
                new Produces(StatusCode: 200,
                    ContentType: typeof(Application.BudgetPermissions.Read.GetBudgetPermission.DTO.Response.
                        GetBudgetPermissionResponse))
            ]);

        EndpointRegistration getBudgetPermissionsEndpointRegistration = new(pattern: "budget-permissions",
            name: "GetBudgetPermissions", accessControl: AccessControl.User, httpVerb: HttpVerb.Get,
            subModule: nameof(BudgetPermission), handler: async (
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
            }, description: "Retrieves a collection of budget permissions filtered by optional parameters.",
            summary: "Retrieve multiple budget permissions with optional filters", responses:
            [
                new Produces(StatusCode: 200, ContentType: typeof(IReadOnlyCollection<GetBudgetPermissionsResponse>))
            ]);

        EndpointRegistration createBudgetPermissionEndpointRegistration = new(pattern: "budget-permissions",
            name: "CreateBudgetPermission", accessControl: AccessControl.User, httpVerb: HttpVerb.Post,
            subModule: nameof(BudgetPermission), handler: async (
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
            }, description: "Creates a new budget permission by providing details in the request body.",
            summary: "Create a new budget permission", responses:
            [
                new Produces(StatusCode: 201, ContentType: typeof(CreateBudgetPermissionResponse))
            ]);

        EndpointRegistration restoreBudgetPermissionEndpointRegistration = new(pattern: "budget-permissions/{id}",
            name: "RestoreBudgetPermission", accessControl: AccessControl.User, httpVerb: HttpVerb.Patch,
            subModule: nameof(BudgetPermission), handler: async (
                [FromServices] ICommandHandler<RestoreBudgetPermissionCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    command: new RestoreBudgetPermissionCommand(MessageContext: messageContextFactory.Current(),
                        BudgetPermissionId: id), cancellationToken: cancellationToken);

                return Results.NoContent();
            }, description: "Restores a previously deleted or archived budget permission.",
            summary: "Restore a deleted or archived budget permission", responses:
            [
                new Produces(StatusCode: 204)
            ]);

        EndpointRegistration deleteBudgetPermissionEndpointRegistration = new(pattern: "budget-permissions/{id}",
            name: "DeleteBudgetPermission", accessControl: AccessControl.User, httpVerb: HttpVerb.Delete,
            subModule: nameof(BudgetPermission), handler: async (
                [FromServices] ICommandHandler<DeleteBudgetPermissionCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    command: new DeleteBudgetPermissionCommand(MessageContext: messageContextFactory.Current(),
                        BudgetPermissionId: id), cancellationToken: cancellationToken);

                return Results.NoContent();
            }, description: "Deletes a budget permission by its unique identifier.",
            summary: "Delete an existing budget permission by Id", responses:
            [
                new Produces(StatusCode: 204)
            ]);

        EndpointRegistration addPermissionEndpointRegistration = new(
            pattern: "budget-permissions/{id}/participants/{participantId}", name: "AddPermission",
            accessControl: AccessControl.User, httpVerb: HttpVerb.Post, subModule: nameof(BudgetPermission),
            handler: async ([FromServices] ICommandHandler<AddPermissionCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                [FromRoute] Guid participantId, [FromBody] AddPermissionRequest addPermissionRequest,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    command: new AddPermissionCommand(MessageContext: messageContextFactory.Current(),
                        BudgetPermissionId: id, ParticipantId: participantId,
                        AddPermissionRequest: addPermissionRequest), cancellationToken: cancellationToken);

                return Results.NoContent();
            }, description: "Adds a new permission for a specific participant to an existing budget permission.",
            summary: "Add a permission for a participant in a budget", responses:
            [
                new Produces(StatusCode: 204)
            ]);

        EndpointRegistration removePermissionEndpointRegistration = new(
            pattern: "budget-permissions/{id}/participants/{participantId}", name: "RemovePermission",
            accessControl: AccessControl.User, httpVerb: HttpVerb.Delete, subModule: nameof(BudgetPermission),
            handler: async ([FromServices] ICommandHandler<RemovePermissionCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                [FromRoute] Guid participantId, CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    command: new RemovePermissionCommand(MessageContext: messageContextFactory.Current(),
                        BudgetPermissionId: id, ParticipantId: participantId), cancellationToken: cancellationToken);

                return Results.NoContent();
            }, description: "Removes a permission associated with a specific participant from a budget permission.",
            summary: "Remove a participant’s permission from a budget", responses:
            [
                new Produces(StatusCode: 204)
            ]);

        return
        [
            getBudgetPermissionEndpointRegistration, getBudgetPermissionsEndpointRegistration,
            createBudgetPermissionEndpointRegistration, restoreBudgetPermissionEndpointRegistration,
            deleteBudgetPermissionEndpointRegistration, addPermissionEndpointRegistration,
            removePermissionEndpointRegistration
        ];
    }
}