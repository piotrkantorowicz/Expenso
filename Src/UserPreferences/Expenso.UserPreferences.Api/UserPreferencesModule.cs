using Expenso.Shared.ModuleDefinition;
using Expenso.UserPreferences.Core;
using Expenso.UserPreferences.Core.DTO.GetUserPreferences;
using Expenso.UserPreferences.Core.DTO.UpdateUserPreferences;
using Expenso.UserPreferences.Core.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.UserPreferences.Api;

public sealed class UserPreferencesModule : ModuleDefinition
{
    public override string ModulePrefix => "/user-preferences";

    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddUserPreferencesModulesDependencies(configuration, ModuleName);
    }

    public override IReadOnlyCollection<EndpointRegistration> CreateEndpoints()
    {
        EndpointRegistration getPreferencesEndpointRegistration = new("preferences/{preferenceId}", "GetPreferences",
            AccessControl.User, HttpVerb.Get, async ([FromRoute] Guid preferenceId,
                [FromServices] IPreferencesService preferencesService, CancellationToken cancellationToken) =>
            {
                PreferenceDto preferences = await preferencesService.GetPreferences(preferenceId, cancellationToken);

                return Results.Ok(preferences);
            });

        EndpointRegistration getCurrentUserPreferencesEndpointRegistration = new("preferences/current-user",
            "GetCurrentUserPreferences", AccessControl.User, HttpVerb.Get,
            async ([FromServices] IPreferencesService preferencesService, CancellationToken cancellationToken) =>
            {
                PreferenceDto preferences =
                    await preferencesService.GetPreferencesForCurrentUserAsync(cancellationToken);

                return Results.Ok(preferences);
            });

        EndpointRegistration getUserPreferencesByEndpointRegistration = new("preferences", "GetUserPreferences",
            AccessControl.User, HttpVerb.Get, async ([FromServices] IPreferencesService preferencesService,
                [FromQuery] Guid userId, CancellationToken cancellationToken) =>
            {
                PreferenceDto preferences =
                    await preferencesService.GetPreferencesForUserAsync(userId, cancellationToken);

                return Results.Ok(preferences);
            });

        EndpointRegistration createPreferencesEndpointRegistration = new("preferences/{userId}", "CreatePreferences",
            AccessControl.User, HttpVerb.Post, async ([FromServices] IPreferencesService preferencesService,
                [FromRoute] Guid userId, CancellationToken cancellationToken) =>
            {
                PreferenceDto preference = await preferencesService.CreatePreferencesAsync(userId, cancellationToken);

                return Results.CreatedAtRoute(getPreferencesEndpointRegistration.Name, new
                {
                    preferenceId = preference.PreferencesId
                }, preference);
            });

        EndpointRegistration updatePreferencesEndpointRegistration = new("preferences/{userOrPreferenceId}",
            "UpdatePreferences",
            AccessControl.User, HttpVerb.Put, async ([FromServices] IPreferencesService preferencesService,
                [FromRoute] Guid userOrPreferenceId, [FromBody] UpdatePreferenceDto model,
                CancellationToken cancellationToken) =>
            {
                await preferencesService.UpdatePreferencesAsync(userOrPreferenceId, model, cancellationToken);

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