using System.Reflection;

using Expenso.Shared.Commands;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Modules;
using Expenso.UserPreferences.Core;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.Internal;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.Internal.DTO.Response;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Internal;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Internal.DTO.Request;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Internal.DTO.Response;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;
using Expenso.UserPreferences.Proxy;

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
                [FromServices] IQueryHandler<GetPreferenceQuery, GetPreferenceResponse> handler, [FromRoute] Guid id,
                [FromQuery] bool? includeFinancePreferences = null,
                [FromQuery] bool? includeNotificationPreferences = null,
                [FromQuery] bool? includeGeneralPreferences = null, CancellationToken cancellationToken = default) =>
            {
                GetPreferenceResponse? getPreferences = await handler.HandleAsync(
                    new GetPreferenceQuery(id, IncludeFinancePreferences: includeFinancePreferences,
                        IncludeNotificationPreferences: includeNotificationPreferences,
                        IncludeGeneralPreferences: includeGeneralPreferences), cancellationToken);

                return Results.Ok(getPreferences);
            });

        EndpointRegistration getCurrentUserPreferencesEndpointRegistration = new("preferences/current-user",
            "GetCurrentUserPreferences", AccessControl.User, HttpVerb.Get, async (
                [FromServices] IQueryHandler<GetPreferenceQuery, GetPreferenceResponse> handler,
                [FromQuery] bool? includeFinancePreferences = null,
                [FromQuery] bool? includeNotificationPreferences = null,
                [FromQuery] bool? includeGeneralPreferences = null, CancellationToken cancellationToken = default) =>
            {
                GetPreferenceResponse? getPreferences = await handler.HandleAsync(
                    new GetPreferenceQuery(ForCurrentUser: true, IncludeFinancePreferences: includeFinancePreferences,
                        IncludeNotificationPreferences: includeNotificationPreferences,
                        IncludeGeneralPreferences: includeGeneralPreferences), cancellationToken);

                return Results.Ok(getPreferences);
            });

        EndpointRegistration getUserPreferencesByEndpointRegistration = new("preferences", "GetUserPreferences",
            AccessControl.User, HttpVerb.Get, async (
                [FromServices] IQueryHandler<GetPreferenceQuery, GetPreferenceResponse> handler,
                [FromQuery] Guid userId, [FromQuery] bool? includeFinancePreferences = null,
                [FromQuery] bool? includeNotificationPreferences = null,
                [FromQuery] bool? includeGeneralPreferences = null, CancellationToken cancellationToken = default) =>
            {
                GetPreferenceResponse? getPreferences = await handler.HandleAsync(
                    new GetPreferenceQuery(UserId: userId, IncludeFinancePreferences: includeFinancePreferences,
                        IncludeNotificationPreferences: includeNotificationPreferences,
                        IncludeGeneralPreferences: includeGeneralPreferences), cancellationToken);

                return Results.Ok(getPreferences);
            });

        EndpointRegistration createPreferencesEndpointRegistration = new("preferences", "CreatePreferences",
            AccessControl.User, HttpVerb.Post,
            async ([FromServices] ICommandHandler<CreatePreferenceCommand, CreatePreferenceResponse> handler,
                [FromBody] CreatePreferenceRequest model, CancellationToken cancellationToken = default) =>
            {
                CreatePreferenceResponse? getPreference = await handler.HandleAsync(
                    new CreatePreferenceCommand(new CreatePreferenceRequest(model.UserId)), cancellationToken);

                return Results.CreatedAtRoute(getPreferencesEndpointRegistration.Name, new
                {
                    id = getPreference?.Id
                }, getPreference);
            });

        EndpointRegistration updatePreferencesEndpointRegistration = new("preferences/{id}", "UpdatePreferences",
            AccessControl.User, HttpVerb.Put, async ([FromServices] ICommandHandler<UpdatePreferenceCommand> handler,
                [FromRoute] Guid id, [FromBody] UpdatePreferenceRequest model,
                CancellationToken cancellationToken = default) =>
            {
                await handler.HandleAsync(new UpdatePreferenceCommand(id, model), cancellationToken);

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