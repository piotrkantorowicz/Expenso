using System.Reflection;

using Expenso.Shared.Commands;
using Expenso.Shared.ModuleDefinition;
using Expenso.Shared.Queries;
using Expenso.UserPreferences.Core;
using Expenso.UserPreferences.Core.Application.Preferences.Commands.CreatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.CreatePreference.Request;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.CreatePreference.Response;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.GetPreferences.Response;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.UpdatePreferences.Request;
using Expenso.UserPreferences.Core.Application.Preferences.Queries.GetPreference;
using Expenso.UserPreferences.Proxy;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RegistrationExtensions = Expenso.UserPreferences.Core.RegistrationExtensions;

namespace Expenso.UserPreferences.Api;

public sealed class UserPreferencesModule : ModuleDefinition
{
    public override string ModulePrefix => "/user-preferences";

    public override Assembly[] GetModuleAssemblies()
    {
        return
        [
            typeof(UserPreferencesModule).Assembly,
            typeof(RegistrationExtensions).Assembly,
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
            AccessControl.User, HttpVerb.Get, async ([FromRoute] Guid id,
                [FromServices] IQueryHandler<GetPreferenceQuery, GetPreferenceResponse> handler,
                CancellationToken cancellationToken = default) =>
            {
                GetPreferenceResponse? getPreferences =
                    await handler.HandleAsync(new GetPreferenceQuery(id), cancellationToken);

                return Results.Ok(getPreferences);
            });

        EndpointRegistration getCurrentUserPreferencesEndpointRegistration = new("preferences/current-user",
            "GetCurrentUserPreferences", AccessControl.User, HttpVerb.Get, async (
                [FromServices] IQueryHandler<GetPreferenceQuery, GetPreferenceResponse> handler,
                CancellationToken cancellationToken = default) =>
            {
                GetPreferenceResponse? getPreferences =
                    await handler.HandleAsync(new GetPreferenceQuery(), cancellationToken);

                return Results.Ok(getPreferences);
            });

        EndpointRegistration getUserPreferencesByEndpointRegistration = new("preferences", "GetUserPreferences",
            AccessControl.User, HttpVerb.Get, async (
                [FromServices] IQueryHandler<GetPreferenceQuery, GetPreferenceResponse> handler,
                [FromQuery] Guid userId, CancellationToken cancellationToken = default) =>
            {
                GetPreferenceResponse? getPreferences =
                    await handler.HandleAsync(new GetPreferenceQuery(UserId: userId), cancellationToken);

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