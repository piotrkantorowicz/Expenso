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

using ServiceCollectionExtensions = Expenso.UserPreferences.Core.ServiceCollectionExtensions;

namespace Expenso.UserPreferences.Api;

public sealed class UserPreferencesModule : ModuleDefinition
{
    public override string ModulePrefix => "/user-preferences";

    public override Assembly[] GetModuleAssemblies()
    {
        return
        [
            typeof(UserPreferencesModule).Assembly,
            typeof(ServiceCollectionExtensions).Assembly,
            typeof(IUserPreferencesProxy).Assembly
        ];
    }

    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddUserPreferencesModulesDependencies(configuration, ModuleName);
    }

    public override IReadOnlyCollection<EndpointRegistration> CreateEndpoints()
    {
        EndpointRegistration getPreferencesEndpointRegistration = new("preferences/{id}", "GetPreferences",
            AccessControl.User, HttpVerb.Get, async (
                [FromServices] IQueryHandler<GetPreferenceQuery, GetPreferenceResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                [FromQuery] bool? includeFinancePreferences = null,
                [FromQuery] bool? includeNotificationPreferences = null,
                [FromQuery] bool? includeGeneralPreferences = null, CancellationToken cancellationToken = default) =>
            {
                GetPreferenceResponse? getPreferences = await handler.HandleAsync(
                    new GetPreferenceQuery(messageContextFactory.Current(), id,
                        IncludeFinancePreferences: includeFinancePreferences,
                        IncludeNotificationPreferences: includeNotificationPreferences,
                        IncludeGeneralPreferences: includeGeneralPreferences), cancellationToken);

                return Results.Ok(getPreferences);
            });

        EndpointRegistration getCurrentUserPreferencesEndpointRegistration = new("preferences/current-user",
            "GetCurrentUserPreferences", AccessControl.User, HttpVerb.Get, async (
                [FromServices] IQueryHandler<GetPreferenceQuery, GetPreferenceResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory,
                [FromQuery] bool? includeFinancePreferences = null,
                [FromQuery] bool? includeNotificationPreferences = null,
                [FromQuery] bool? includeGeneralPreferences = null, CancellationToken cancellationToken = default) =>
            {
                GetPreferenceResponse? getPreferences = await handler.HandleAsync(
                    new GetPreferenceQuery(messageContextFactory.Current(), ForCurrentUser: true,
                        IncludeFinancePreferences: includeFinancePreferences,
                        IncludeNotificationPreferences: includeNotificationPreferences,
                        IncludeGeneralPreferences: includeGeneralPreferences), cancellationToken);

                return Results.Ok(getPreferences);
            });

        EndpointRegistration getUserPreferencesByEndpointRegistration = new("preferences", "GetUserPreferences",
            AccessControl.User, HttpVerb.Get, async (
                [FromServices] IQueryHandler<GetPreferenceQuery, GetPreferenceResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromQuery] Guid userId,
                [FromQuery] bool? includeFinancePreferences = null,
                [FromQuery] bool? includeNotificationPreferences = null,
                [FromQuery] bool? includeGeneralPreferences = null, CancellationToken cancellationToken = default) =>
            {
                GetPreferenceResponse? getPreferences = await handler.HandleAsync(
                    new GetPreferenceQuery(messageContextFactory.Current(), UserId: userId,
                        IncludeFinancePreferences: includeFinancePreferences,
                        IncludeNotificationPreferences: includeNotificationPreferences,
                        IncludeGeneralPreferences: includeGeneralPreferences), cancellationToken);

                return Results.Ok(getPreferences);
            });

        EndpointRegistration createPreferencesEndpointRegistration = new("preferences", "CreatePreferences",
            AccessControl.User, HttpVerb.Post, async (
                [FromServices] ICommandHandler<CreatePreferenceCommand, CreatePreferenceResponse> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromBody] CreatePreferenceRequest model,
                CancellationToken cancellationToken = default) =>
            {
                CreatePreferenceResponse? createPreferenceResponse = await handler.HandleAsync(
                    new CreatePreferenceCommand(messageContextFactory.Current(),
                        new CreatePreferenceRequest(model.UserId)), cancellationToken);

                return Results.CreatedAtRoute(getPreferencesEndpointRegistration.Name, new
                {
                    id = createPreferenceResponse?.PreferenceId
                }, createPreferenceResponse);
            });

        EndpointRegistration updatePreferencesEndpointRegistration = new("preferences/{id}", "UpdatePreferences",
            AccessControl.User, HttpVerb.Put, async ([FromServices] ICommandHandler<UpdatePreferenceCommand> handler,
                [FromServices] IMessageContextFactory messageContextFactory, [FromRoute] Guid id,
                [FromBody] UpdatePreferenceRequest model, CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(new UpdatePreferenceCommand(messageContextFactory.Current(), id, model),
                    cancellationToken);

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