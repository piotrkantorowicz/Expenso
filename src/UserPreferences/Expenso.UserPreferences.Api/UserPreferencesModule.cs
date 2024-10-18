using System.Reflection;

using Expenso.Shared.Commands;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Modules;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Core;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Request;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Response;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser.DTO.Request;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser.DTO.Response;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferences;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;
using Expenso.UserPreferences.Shared;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Response;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Request;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Extensions = Expenso.UserPreferences.Core.Extensions;

namespace Expenso.UserPreferences.Api;

public sealed class UserPreferencesModule : IModuleDefinition
{
    public string ModulePrefix => "/user-preferences";

    public IReadOnlyCollection<Assembly> GetModuleAssemblies()
    {
        return
        [
            typeof(UserPreferencesModule).Assembly,
            typeof(Extensions).Assembly,
            typeof(IUserPreferencesProxy).Assembly
        ];
    }

    public void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddUserPreferencesCore(configuration: configuration, moduleName: GetType().Name);
        services.AddUserPreferencesProxy(assemblies: GetModuleAssemblies());
    }

    public IReadOnlyCollection<EndpointRegistration> CreateEndpoints()
    {
        EndpointRegistration getPreferenceEndpointRegistration = new(Pattern: "preferences/{id}", Name: "GetPreference",
            AccessControl: AccessControl.User, HttpVerb: HttpVerb.Get, Handler: async (
                [FromServices] IQueryHandler<GetPreferenceQuery, GetPreferenceResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                [FromQuery] GetPreferenceRequest_PreferenceTypes preferenceType =
                    GetPreferenceRequest_PreferenceTypes.None, CancellationToken cancellationToken = default) =>
            {
                GetPreferenceResponse? response = await handler.HandleAsync(
                    query: new GetPreferenceQuery(MessageContext: messageContextFactory.Current(),
                        Payload: new GetPreferenceRequest(PreferenceId: id, PreferenceType: preferenceType)),
                    cancellationToken: cancellationToken);

                return Results.Ok(value: response);
            });

        EndpointRegistration getCurrentUserPreferencesEndpointRegistration = new(Pattern: "preferences/current-user",
            Name: "GetCurrentUserPreferences", AccessControl: AccessControl.User, HttpVerb: HttpVerb.Get,
            Handler: async (
                [FromServices]
                IQueryHandler<GetPreferenceForCurrentUserQuery, GetPreferenceForCurrentUserResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory,
                [FromQuery] GetPreferenceForCurrentUserRequest_PreferenceTypes preferenceType =
                    GetPreferenceForCurrentUserRequest_PreferenceTypes.None,
                CancellationToken cancellationToken = default) =>
            {
                GetPreferenceForCurrentUserResponse? response = await handler.HandleAsync(
                    query: new GetPreferenceForCurrentUserQuery(MessageContext: messageContextFactory.Current(),
                        Payload: new GetPreferenceForCurrentUserRequest(PreferenceType: preferenceType)),
                    cancellationToken: cancellationToken);

                return Results.Ok(value: response);
            });

        EndpointRegistration getPreferencesEndpointRegistration = new(Pattern: "preferences", Name: "GetPreferences",
            AccessControl: AccessControl.User, HttpVerb: HttpVerb.Get, Handler: async (
                [FromServices] IQueryHandler<GetPreferencesQuery, GetPreferencesResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromQuery] Guid? id = null,
                [FromQuery] Guid? userId = null,
                [FromQuery] GetPreferencesRequest_PreferenceTypes preferenceType =
                    GetPreferencesRequest_PreferenceTypes.None, CancellationToken cancellationToken = default) =>
            {
                GetPreferencesResponse? response = await handler.HandleAsync(
                    query: new GetPreferencesQuery(MessageContext: messageContextFactory.Current(),
                        Payload: new GetPreferencesRequest(PreferenceId: id, UserId: userId,
                            PreferenceType: preferenceType)), cancellationToken: cancellationToken);

                return Results.Ok(value: response);
            });

        EndpointRegistration createPreferencesEndpointRegistration = new(Pattern: "preferences",
            Name: "CreatePreferences", AccessControl: AccessControl.User, HttpVerb: HttpVerb.Post, Handler: async (
                [FromServices] ICommandHandler<CreatePreferenceCommand, CreatePreferenceResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromBody] CreatePreferenceRequest model,
                CancellationToken cancellationToken = default) =>
            {
                CreatePreferenceResponse response = await handler.HandleAsync(
                    command: new CreatePreferenceCommand(MessageContext: messageContextFactory.Current(),
                        Payload: new CreatePreferenceRequest(UserId: model.UserId)),
                    cancellationToken: cancellationToken);

                return Results.CreatedAtRoute(routeName: getPreferenceEndpointRegistration.Name, routeValues: new
                {
                    id = response.PreferenceId
                }, value: response);
            });

        EndpointRegistration updatePreferencesEndpointRegistration = new(Pattern: "preferences/{id}",
            Name: "UpdatePreferences", AccessControl: AccessControl.User, HttpVerb: HttpVerb.Put, Handler: async (
                [FromServices] ICommandHandler<UpdatePreferenceCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                [FromBody] UpdatePreferenceRequest model, CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    command: new UpdatePreferenceCommand(MessageContext: messageContextFactory.Current(),
                        PreferenceId: id, Payload: model), cancellationToken: cancellationToken);

                return Results.NoContent();
            });

        return
        [
            getPreferenceEndpointRegistration,
            getCurrentUserPreferencesEndpointRegistration,
            getPreferencesEndpointRegistration,
            createPreferencesEndpointRegistration,
            updatePreferencesEndpointRegistration
        ];
    }
}