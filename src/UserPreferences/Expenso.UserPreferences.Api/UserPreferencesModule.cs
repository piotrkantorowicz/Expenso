using System.Reflection;

using Expenso.Shared.Commands;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Modules;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Api.Swagger.SchemaFilters.CreatePreference.Request;
using Expenso.UserPreferences.Api.Swagger.SchemaFilters.CreatePreference.Response;
using Expenso.UserPreferences.Api.Swagger.SchemaFilters.GetPreference.Response;
using Expenso.UserPreferences.Api.Swagger.SchemaFilters.UpdatePreference.Request;
using Expenso.UserPreferences.Core;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Requests;
using Expenso.UserPreferences.Proxy;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Requests;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Responses;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Responses;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Swashbuckle.AspNetCore.SwaggerGen;

using CoreExtensions = Expenso.UserPreferences.Core.Extensions;

namespace Expenso.UserPreferences.Api;

public sealed class UserPreferencesModule : IModuleDefinition
{
    public string ModulePrefix => "/user-preferences";

    public IReadOnlyCollection<Assembly> GetModuleAssemblies()
    {
        return
        [
            typeof(UserPreferencesModule).Assembly,
            typeof(CoreExtensions).Assembly,
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
        EndpointRegistration getPreferencesEndpointRegistration = new(pattern: "preferences/{id}",
            name: "GetPreferences", accessControl: AccessControl.User, httpVerb: HttpVerb.Get, handler: async (
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
            },
            description:
            "Retrieves the preferences of a specific user by the provided preference Id. Optional query parameters allow filtering the preferences based on finance, notification, or general preferences.",
            summary: "Fetches preferences by preference Id", responses:
            [
                new Produces(StatusCode: StatusCodes.Status200OK, ContentType: typeof(GetPreferenceResponse))
            ]);

        EndpointRegistration getCurrentUserPreferencesEndpointRegistration = new(pattern: "preferences/current-user",
            name: "GetCurrentUserPreferences", accessControl: AccessControl.User, httpVerb: HttpVerb.Get,
            handler: async ([FromServices] IQueryHandler<GetPreferenceQuery, GetPreferenceResponse> handler,
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
            },
            description:
            "Retrieves the current user's preferences. Optional query parameters allow filtering based on finance, notification, or general preferences.",
            summary: "Fetches preferences for the current user", responses:
            [
                new Produces(StatusCode: StatusCodes.Status200OK, ContentType: typeof(GetPreferenceResponse))
            ]);

        EndpointRegistration getPreferencesByUserIdEndpointRegistration = new(pattern: "preferences",
            name: "GetUserPreferences", accessControl: AccessControl.User, httpVerb: HttpVerb.Get, handler: async (
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
            },
            description:
            "Retrieves the preferences of a user by their user Id. The response can optionally include financial, notification, or general preferences based on the query parameters.",
            summary: "Fetches preferences by user Id", responses:
            [
                new Produces(StatusCode: StatusCodes.Status200OK, ContentType: typeof(GetPreferenceResponse))
            ]);

        EndpointRegistration createPreferencesEndpointRegistration = new(pattern: "preferences",
            name: "CreatePreferences", accessControl: AccessControl.User, httpVerb: HttpVerb.Post, handler: async (
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
            },
            description:
            "Creates new preferences for a user. The user Id is provided in the request body, and upon success, the created preferences are returned along with the preference Id.",
            summary: "Creates a new set of user preferences", responses:
            [
                new Produces(StatusCode: StatusCodes.Status201Created, ContentType: typeof(CreatePreferenceResponse))
            ]);

        EndpointRegistration updatePreferencesEndpointRegistration = new(pattern: "preferences/{id}",
            name: "UpdatePreferences", accessControl: AccessControl.User, httpVerb: HttpVerb.Put, handler: async (
                [FromServices] ICommandHandler<UpdatePreferenceCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                [FromBody] UpdatePreferenceRequest model, CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(
                    command: new UpdatePreferenceCommand(MessageContext: messageContextFactory.Current(),
                        PreferenceOrUserId: id, Preference: model), cancellationToken: cancellationToken);

                return Results.NoContent();
            },
            description:
            "Updates the preferences for a user based on the provided preference Id. The request body should contain the updated preference data.",
            summary: "Updates existing user preferences", responses:
            [
                new Produces(StatusCode: StatusCodes.Status204NoContent, ContentType: typeof(void))
            ]);

        return
        [
            getPreferencesEndpointRegistration,
            getCurrentUserPreferencesEndpointRegistration,
            getPreferencesByUserIdEndpointRegistration,
            createPreferencesEndpointRegistration,
            updatePreferencesEndpointRegistration
        ];
    }

    public void ConfigureSwaggerOptions(SwaggerGenOptions options)
    {
        options.SchemaFilter<CreatePreferenceRequestSchemaFilter>();
        options.SchemaFilter<CreatePreferenceResponseSchemaFilter>();
        options.SchemaFilter<GetPreferenceResponseSchemaFilter>();
        options.SchemaFilter<GetPreferenceResponseGeneralPreferenceSchemaFilter>();
        options.SchemaFilter<GetPreferenceResponseFinancePreferenceSchemaFilter>();
        options.SchemaFilter<GetPreferenceResponseNotificationPreferenceSchemaFilter>();
        options.SchemaFilter<UpdatePreferenceRequestSchemaFilter>();
        options.SchemaFilter<UpdatePreferenceRequestFinancePreferenceSchemaFilter>();
        options.SchemaFilter<UpdatePreferenceRequestNotificationPreferenceSchemaFilter>();
        options.SchemaFilter<UpdatePreferenceRequestGeneralPreferenceSchemaFilter>();
    }
}