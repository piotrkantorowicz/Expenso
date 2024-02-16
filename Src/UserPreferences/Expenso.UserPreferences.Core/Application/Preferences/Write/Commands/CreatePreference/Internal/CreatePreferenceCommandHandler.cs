using Expenso.Shared.Commands;
using Expenso.Shared.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Internal.DTO.Maps;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Internal.DTO.Response;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Shared.Factories;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Internal;

internal sealed class CreatePreferenceCommandHandler(IPreferencesRepository preferencesRepository)
    : ICommandHandler<CreatePreferenceCommand, CreatePreferenceResponse>
{
    private readonly IPreferencesRepository _preferencesRepository =
        preferencesRepository ?? throw new ArgumentNullException(nameof(preferencesRepository));

    public async Task<CreatePreferenceResponse?> HandleAsync(CreatePreferenceCommand command,
        CancellationToken cancellationToken = default)
    {
        PreferenceFilter filter = new()
        {
            UserId = command.Preference.UserId,
            UseTracking = false
        };

        bool dbUserPreferencesExists = await _preferencesRepository.ExistsAsync(filter, cancellationToken);

        if (dbUserPreferencesExists)
        {
            throw new ConflictException($"Preferences for user with id {command.Preference.UserId} already exists");
        }

        Preference preferenceToCreate = PreferenceFactory.Create(command.Preference.UserId);
        Preference preference = await _preferencesRepository.CreateAsync(preferenceToCreate, cancellationToken);

        return CreatePreferenceResponseMap.MapTo(preference);
    }
}