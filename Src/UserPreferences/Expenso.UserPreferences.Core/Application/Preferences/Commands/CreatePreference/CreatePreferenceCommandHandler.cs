using Expenso.Shared.Commands;
using Expenso.Shared.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.CreatePreference.Request;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.CreatePreference.Response;
using Expenso.UserPreferences.Core.Application.Preferences.Mappings;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;

namespace Expenso.UserPreferences.Core.Application.Preferences.Commands.CreatePreference;

internal sealed class CreatePreferenceCommandHandler(IPreferencesRepository preferencesRepository)
    : ICommandHandler<CreatePreferenceCommand, CreatePreferenceResponse>
{
    private readonly IPreferencesRepository _preferencesRepository =
        preferencesRepository ?? throw new ArgumentNullException(nameof(preferencesRepository));

    public async Task<CreatePreferenceResponse?> HandleAsync(CreatePreferenceCommand command,
        CancellationToken cancellationToken = default)
    {
        CreatePreferenceRequest request = command.Preference;

        Preference? dbUserPreferences =
            await _preferencesRepository.GetByUserIdAsync(request.UserId, true, cancellationToken);

        if (dbUserPreferences is not null)
        {
            throw new ConflictException($"Preferences for user with id {request.UserId} already exists.");
        }

        Preference preferenceToCreate = Preference.CreateDefault(request.UserId);
        Preference preference = await _preferencesRepository.CreateAsync(preferenceToCreate, cancellationToken);

        return PreferenceMap.MapToCreateResponse(preference);
    }
}
