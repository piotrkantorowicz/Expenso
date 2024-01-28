using Expenso.Shared.Commands;
using Expenso.Shared.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.Mappings;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

namespace Expenso.UserPreferences.Core.Application.Preferences.Internal.Commands.CreatePreference;

internal sealed class CreatePreferenceInternalCommandHandler(IPreferencesRepository preferencesRepository)
    : ICommandHandler<CreatePreferenceInternalCommand, CreatePreferenceInternalResponse>
{
    private readonly IPreferencesRepository _preferencesRepository =
        preferencesRepository ?? throw new ArgumentNullException(nameof(preferencesRepository));

    public async Task<CreatePreferenceInternalResponse?> HandleAsync(CreatePreferenceInternalCommand command,
        CancellationToken cancellationToken = default)
    {
        CreatePreferenceInternalRequest request = command.Preference;

        Preference? dbUserPreferences =
            await _preferencesRepository.GetByUserIdAsync(request.UserId, true, cancellationToken);

        if (dbUserPreferences is not null)
        {
            throw new ConflictException($"Preferences for user with id {request.UserId} already exists.");
        }

        Preference preferenceToCreate = Preference.CreateDefault(request.UserId);
        Preference preference = await _preferencesRepository.CreateAsync(preferenceToCreate, cancellationToken);

        return PreferenceMap.MapToInternalCreateResponse(preference);
    }
}