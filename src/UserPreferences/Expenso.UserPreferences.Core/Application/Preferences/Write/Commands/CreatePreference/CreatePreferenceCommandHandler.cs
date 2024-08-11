using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.DTO.Maps;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Factories;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;

internal sealed class CreatePreferenceCommandHandler(IPreferencesRepository preferencesRepository)
    : ICommandHandler<CreatePreferenceCommand, CreatePreferenceResponse>
{
    private readonly IPreferencesRepository _preferencesRepository = preferencesRepository ??
                                                                     throw new ArgumentNullException(
                                                                         paramName: nameof(preferencesRepository));

    public async Task<CreatePreferenceResponse?> HandleAsync(CreatePreferenceCommand command,
        CancellationToken cancellationToken)
    {
        PreferenceFilter filter = new()
        {
            UserId = command.Preference.UserId,
            UseTracking = false
        };

        bool dbUserPreferencesExists =
            await _preferencesRepository.ExistsAsync(preferenceFilter: filter, cancellationToken: cancellationToken);

        if (dbUserPreferencesExists)
        {
            throw new ConflictException(
                message: $"Preferences for user with id {command.Preference.UserId} already exists");
        }

        Preference preferenceToCreate = PreferenceFactory.Create(userId: command.Preference.UserId);

        Preference preference =
            await _preferencesRepository.CreateAsync(preference: preferenceToCreate,
                cancellationToken: cancellationToken);

        return CreatePreferenceResponseMap.MapTo(preference: preference);
    }
}