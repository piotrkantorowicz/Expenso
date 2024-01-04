using Expenso.Shared.ModuleDefinition;
using Expenso.UserPreferences.Core;
using Expenso.UserPreferences.Core.DTO.GetUserPreferences;
using Expenso.UserPreferences.Core.DTO.UpdateUserPreferences;
using Expenso.UserPreferences.Core.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.UserPreferences.Api;

public sealed class UserPreferencesModule : ModuleDefinition
{
    public override string ModulePrefix => "/user-preferences";

    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddUserPreferences(configuration);
    }

    public override IReadOnlyCollection<EndpointRegistration> CreateEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        EndpointRegistration getCurrentUserPreferencesEndpointRegistration = new("preferences/current-user",
            "GetCurrentUserPreferences", AccessControl.User, HttpVerb.Get,
            async ([FromServices] IPreferencesService preferencesService, CancellationToken cancellationToken) =>
            {
                PreferenceDto preferences =
                    await preferencesService.GetPreferencesForCurrentUserAsync(cancellationToken);

                return Results.Ok(preferences);
            });

        EndpointRegistration getPreferencesEndpointRegistration = new("preferences/{userId}", "GetPreferences",
            AccessControl.User, HttpVerb.Get, async ([FromServices] IPreferencesService preferencesService,
                [FromRoute] Guid userId, CancellationToken cancellationToken) =>
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
                    userId = preference.PreferencesId
                }, preference);
            });

        EndpointRegistration updatePreferencesEndpointRegistration = new("preferences/{userId}", "UpdatePreferences",
            AccessControl.User, HttpVerb.Put, async ([FromServices] IPreferencesService preferencesService,
                [FromRoute] Guid userId, [FromBody] UpdatePreferenceDto model, CancellationToken cancellationToken) =>
            {
                await preferencesService.UpdatePreferencesAsync(userId, model, cancellationToken);

                return Results.NoContent();
            });

        return new[]
        {
            getCurrentUserPreferencesEndpointRegistration,
            getPreferencesEndpointRegistration,
            createPreferencesEndpointRegistration,
            updatePreferencesEndpointRegistration
        };
    }
}