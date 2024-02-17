using System.Reflection;

using Expenso.BudgetSharing.Application.Read.GetBudgetPermission;
using Expenso.BudgetSharing.Application.Read.GetBudgetPermission.DTO.Responses;
using Expenso.BudgetSharing.Application.Read.GetBudgetPermissionRequest;
using Expenso.BudgetSharing.Application.Read.GetBudgetPermissionRequest.DTO.Responses;
using Expenso.BudgetSharing.Application.Read.GetBudgetPermissionRequests;
using Expenso.BudgetSharing.Application.Read.GetBudgetPermissionRequests.DTO.Requests;
using Expenso.BudgetSharing.Application.Read.GetBudgetPermissionRequests.DTO.Responses;
using Expenso.BudgetSharing.Application.Read.GetBudgetPermissions;
using Expenso.BudgetSharing.Application.Read.GetBudgetPermissions.DTO.Requests;
using Expenso.BudgetSharing.Application.Read.GetBudgetPermissions.DTO.Responses;
using Expenso.BudgetSharing.Application.Read.Shared.QueryStore;
using Expenso.BudgetSharing.Application.Write.AssignParticipant;
using Expenso.BudgetSharing.Application.Write.AssignParticipant.DTO.Reponses;
using Expenso.BudgetSharing.Application.Write.AssignParticipant.DTO.Requests;
using Expenso.BudgetSharing.Application.Write.CancelAssigningParticipant;
using Expenso.BudgetSharing.Application.Write.ConfirmAssigningParticipat;
using Expenso.BudgetSharing.Application.Write.RemovePermission;
using Expenso.BudgetSharing.Domain;
using Expenso.BudgetSharing.Infrastructure;
using Expenso.BudgetSharing.Proxy;
using Expenso.Shared.Commands;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Modules;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using DomainRegistrationExtensions = Expenso.BudgetSharing.Domain.RegistrationExtensions;
using InfrastructureRegistrationExtensions = Expenso.BudgetSharing.Infrastructure.RegistrationExtensions;

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
            typeof(InfrastructureRegistrationExtensions).Assembly,
            typeof(DomainRegistrationExtensions).Assembly,
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
                [FromRoute] Guid id, CancellationToken cancellationToken = default) =>
            {
                GetBudgetPermissionRequestResponse? getPreferences =
                    await handler.HandleAsync(new GetBudgetPermissionRequestQuery(id), cancellationToken);

                return Results.Ok(getPreferences);
            });

        EndpointRegistration getBudgetPermissionRequestsEndpointRegistration = new("budget-permission-requests",
            "GetBudgetPermissionRequests", AccessControl.User, HttpVerb.Get, async (
                [FromServices]
                IQueryHandler<GetBudgetPermissionRequestsQuery,
                    IReadOnlyCollection<GetBudgetPermissionRequestsResponse>> handler,
                [FromQuery] Guid? budgetId = null, [FromQuery] Guid? participantId = null,
                [FromQuery] bool? forCurrentUser = null,
                [FromQuery] GetBudgetPermissionRequestsRequestStatus? status = null,
                [FromQuery] GetBudgetPermissionRequestsRequestPermissionType? permissionType = null,
                CancellationToken cancellationToken = default) =>
            {
                IReadOnlyCollection<GetBudgetPermissionRequestsResponse>? getPreferences = await handler.HandleAsync(
                    new GetBudgetPermissionRequestsQuery(budgetId, participantId, forCurrentUser, status,
                        permissionType), cancellationToken);

                return Results.Ok(getPreferences);
            });

        EndpointRegistration assignParticipantEndpointRegistration = new("budget-permission-requests",
            "AssignParticipant", AccessControl.User, HttpVerb.Post, async (
                [FromServices] ICommandHandler<AssignParticipantCommand, AssignParticipantResponse> handler,
                [FromBody] AssignParticipantRequest assignParticipantRequest,
                CancellationToken cancellationToken = default) =>
            {
                AssignParticipantResponse? response = await handler.HandleAsync(
                    new AssignParticipantCommand(assignParticipantRequest), cancellationToken);

                return Results.CreatedAtRoute(getBudgetPermissionRequestEndpointRegistration.Name, new
                {
                    id = response
                });
            });

        EndpointRegistration confirmAssigningParticipantEndpointRegistration = new(
            "budget-permission-requests/{id}/confirm", "ConfirmAssigningParticipant", AccessControl.User,
            HttpVerb.Patch, async ([FromServices] ICommandHandler<ConfirmAssigningParticipantCommand> handler,
                [FromRoute] Guid id, CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(new ConfirmAssigningParticipantCommand(id), cancellationToken);

                return Results.NoContent();
            });

        EndpointRegistration cancelAssigningParticipantEndpointRegistration = new(
            "budget-permission-requests/{id}/cancel", "CancelAssigningParticipant", AccessControl.User, HttpVerb.Patch,
            async ([FromServices] ICommandHandler<CancelAssigningParticipantCommand> handler, [FromRoute] Guid id,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(new CancelAssigningParticipantCommand(id), cancellationToken);

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
                [FromRoute] Guid id, [FromQuery] bool includePermissions = false,
                CancellationToken cancellationToken = default) =>
            {
                GetBudgetPermissionResponse? getPreferences = await handler.HandleAsync(
                    new GetBudgetPermissionQuery(id, includePermissions), cancellationToken);

                return Results.Ok(getPreferences);
            });

        EndpointRegistration getBudgetPermissionsEndpointRegistration = new("budget-permissions",
            "GetBudgetPermissions", AccessControl.User, HttpVerb.Get, async (
                [FromServices]
                IQueryHandler<GetBudgetPermissionsQuery, IReadOnlyCollection<GetBudgetPermissionsResponse>> handler,
                [FromQuery] Guid? budgetId = null, [FromQuery] Guid? ownerId = null,
                [FromQuery] Guid? participantId = null, [FromQuery] bool? forCurrentUser = null,
                [FromQuery] GetBudgetPermissionsRequestPermissionType? permissionType = null,
                [FromQuery] bool includePermissions = false, CancellationToken cancellationToken = default) =>
            {
                IReadOnlyCollection<GetBudgetPermissionsResponse>? getPreferences = await handler.HandleAsync(
                    new GetBudgetPermissionsQuery(budgetId, ownerId, participantId, PermissionType: permissionType,
                        ForCurrentUser: forCurrentUser, IncludePermissions: includePermissions), cancellationToken);

                return Results.Ok(getPreferences);
            });

        EndpointRegistration removePermissionEndpointRegistration = new(
            "budget-permissions/{id}/permissions/{participantId}", "RemovePermission", AccessControl.User,
            HttpVerb.Delete, async ([FromServices] ICommandHandler<RemovePermissionCommand> handler,
                [FromRoute] Guid id, [FromRoute] Guid participantId, CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(new RemovePermissionCommand(id, participantId), cancellationToken);

                return Results.NoContent();
            });

        return
        [
            getBudgetPermissionEndpointRegistration, getBudgetPermissionsEndpointRegistration,
            removePermissionEndpointRegistration
        ];
    }
}