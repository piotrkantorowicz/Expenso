using System.Reflection;

using Expenso.Shared.Commands;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Modules;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Core;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;
using Expenso.UserPreferences.Proxy;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using CoreExtensions = Expenso.UserPreferences.Core.Extensions;

namespace Expenso.UserPreferences.Api;

public sealed class UserPreferencesModule : ModuleDefinition
{
    public override string ModulePrefix => "/user-preferences";

    public override Assembly[] GetModuleAssemblies()
    {
        return
        [
            typeof(UserPreferencesModule).Assembly,
            typeof(CoreExtensions).Assembly,
            typeof(IUserPreferencesProxy).Assembly
        ];
    }

    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddUserPreferencesCore(configuration: configuration, moduleName: ModuleName);
        services.AddUserPreferencesProxy(assemblies: GetModuleAssemblies());
    }

    public override IReadOnlyCollection<EndpointRegistration> CreateEndpoints()
    {
        EndpointRegistration getPreferencesEndpointRegistration = new(Pattern: "preferences/{id}",
            Name: "GetPreferences", AccessControl: AccessControl.User, HttpVerb: HttpVerb.Get, Handler: async (
                [FromServices] IQueryHandler<GetPreferenceQuery, GetPreferenceResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                [FromQuery] bool? includeFinancePreferences = null,
                [FromQuery] bool? includeNotificationPreferences = null,
                [FromQuery] bool? includeGeneralPreferences = null, CancellationToken cancellationToken = default) =>
            {
                GetPreferenceResponse? getPreferences = await handler.HandleAsync(
                    query: new GetPreferenceQuery(MessageContext: messageContextFactory.Current(), PreferenceId: id,
                        IncludeFinancePreferences: includeFinancePreferences,
                        IncludeNotificationPreferences: includeNotificationPreferences,
                        IncludeGeneralPreferences: includeGeneralPreferences), cancellationToken: cancellationToken);

                return Results.Ok(value: getPreferences);
            });

        EndpointRegistration getCurrentUserPreferencesEndpointRegistration = new(Pattern: "preferences/current-user",
            Name: "GetCurrentUserPreferences", AccessControl: AccessControl.User, HttpVerb: HttpVerb.Get,
            Handler: async ([FromServices] IQueryHandler<GetPreferenceQuery, GetPreferenceResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory,
                [FromQuery] bool? includeFinancePreferences = null,
                [FromQuery] bool? includeNotificationPreferences = null,
                [FromQuery] bool? includeGeneralPreferences = null, CancellationToken cancellationToken = default) =>
            {
                GetPreferenceResponse? getPreferences = await handler.HandleAsync(
                    query: new GetPreferenceQuery(MessageContext: messageContextFactory.Current(), ForCurrentUser: true,
                        IncludeFinancePreferences: includeFinancePreferences,
                        IncludeNotificationPreferences: includeNotificationPreferences,
                        IncludeGeneralPreferences: includeGeneralPreferences), cancellationToken: cancellationToken);

                return Results.Ok(value: getPreferences);
            });

        EndpointRegistration getUserPreferencesByEndpointRegistration = new(Pattern: "preferences",
            Name: "GetUserPreferences", AccessControl: AccessControl.User, HttpVerb: HttpVerb.Get, Handler: async (
                [FromServices] IQueryHandler<GetPreferenceQuery, GetPreferenceResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromQuery] Guid userId,
                [FromQuery] bool? includeFinancePreferences = null,
                [FromQuery] bool? includeNotificationPreferences = null,
                [FromQuery] bool? includeGeneralPreferences = null, CancellationToken cancellationToken = default) =>
            {
                GetPreferenceResponse? getPreferences = await handler.HandleAsync(
                    query: new GetPreferenceQuery(MessageContext: messageContextFactory.Current(), UserId: userId,
                        IncludeFinancePreferences: includeFinancePreferences,
                        IncludeNotificationPreferences: includeNotificationPreferences,
                        IncludeGeneralPreferences: includeGeneralPreferences), cancellationToken: cancellationToken);

                return Results.Ok(value: getPreferences);
            });

        EndpointRegistration createPreferencesEndpointRegistration = new(Pattern: "preferences",
            Name: "CreatePreferences", AccessControl: AccessControl.User, HttpVerb: HttpVerb.Post, Handler: async (
                [FromServices] ICommandHandler<CreatePreferenceCommand, CreatePreferenceResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromBody] CreatePreferenceRequest model,
                CancellationToken cancellationToken = default) =>
            {
                CreatePreferenceResponse? createPreferenceResponse = await handler.HandleAsync(
                    command: new CreatePreferenceCommand(MessageContext: messageContextFactory.Current(),
                        Preference: new CreatePreferenceRequest(UserId: model.UserId)),
                    cancellationToken: cancellationToken);

                return Results.CreatedAtRoute(routeName: getPreferencesEndpointRegistration.Name, routeValues: new
                {
                    id = createPreferenceResponse?.PreferenceId
                }, value: createPreferenceResponse);
            });

        EndpointRegistration updatePreferencesEndpointRegistration = new(Pattern: "preferences/{id}",
            Name: "UpdatePreferences", AccessControl: AccessControl.User, HttpVerb: HttpVerb.Put, Handler: async (
                [FromServices] ICommandHandler<UpdatePreferenceCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                [FromBody] UpdatePreferenceRequest model, CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    command: new UpdatePreferenceCommand(MessageContext: messageContextFactory.Current(),
                        PreferenceOrUserId: id, Preference: model), cancellationToken: cancellationToken);

                return Results.NoContent();
            });

        return new[]
        {
            getPreferencesEndpointRegistration,
            getCurrentUserPreferencesEndpointRegistration,
            getUserPreferencesByEndpointRegistration,
            createPreferencesEndpointRegistration,
            updatePreferencesEndpointRegistration
        };
    }
}