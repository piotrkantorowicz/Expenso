using Expenso.Shared.MessageBroker;
using Expenso.Shared.Types.Exceptions;
using Expenso.Shared.UserContext;
using Expenso.UserPreferences.Core.DTO.GetUserPreferences;
using Expenso.UserPreferences.Core.DTO.UpdateUserPreferences;
using Expenso.UserPreferences.Core.Mappings;
using Expenso.UserPreferences.Core.Models;
using Expenso.UserPreferences.Core.Repositories;
using Expenso.UserPreferences.Core.Validators;
using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.UserPreferences.Core.Services;

internal sealed class PreferencesService(
    IPreferencesRepository preferencesRepository,
    IUserContextAccessor userContextAccessor,
    IMessageBroker messageBroker,
    IPreferenceValidator preferenceValidator) : IPreferencesService
{
    private readonly IMessageBroker _messageBroker =
        messageBroker ?? throw new ArgumentNullException(nameof(messageBroker));

    private readonly IPreferencesRepository _preferencesRepository =
        preferencesRepository ?? throw new ArgumentNullException(nameof(preferencesRepository));

    private readonly IPreferenceValidator _preferenceValidator =
        preferenceValidator ?? throw new ArgumentNullException(nameof(preferenceValidator));

    private readonly IUserContextAccessor _userContextAccessor =
        userContextAccessor ?? throw new ArgumentNullException(nameof(userContextAccessor));

    public async Task<PreferenceDto> GetPreferences(Guid preferenceId, CancellationToken cancellationToken)
    {
        Preference preference = await _preferencesRepository.GetByIdAsync(preferenceId, false, cancellationToken) ??
                                throw new NotFoundException($"Preferences with id {preferenceId} not found.");

        return PreferenceMap.MapToDto(preference);
    }

    public Task<PreferenceDto> GetPreferencesForCurrentUserAsync(CancellationToken cancellationToken)
    {
        Guid userId = Guid.TryParse(_userContextAccessor.Get()?.UserId, out Guid id) ? id : Guid.Empty;

        return GetPreferencesForUserAsync(userId, cancellationToken);
    }

    public async Task<PreferenceDto> GetPreferencesForUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        Preference preference = await _preferencesRepository.GetByUserIdAsync(userId, false, cancellationToken) ??
                                throw new NotFoundException(userId == Guid.Empty
                                    ? "Preferences for user not found."
                                    : $"Preferences for user with id {userId} not found.");

        return PreferenceMap.MapToDto(preference);
    }

    public async Task<PreferenceContract> GetPreferencesForUserInternalAsync(Guid userId,
        CancellationToken cancellationToken)
    {
        Preference preference = await _preferencesRepository.GetByUserIdAsync(userId, false, cancellationToken) ??
                                throw new NotFoundException($"Preferences for user with id {userId} not found.");

        return PreferenceMap.MapToContract(preference);
    }

    public async Task<PreferenceDto> CreatePreferencesAsync(Guid userId, CancellationToken cancellationToken)
    {
        await _preferenceValidator.ValidateCreateAsync(userId, cancellationToken);
        Preference userPreferenceToCreate = Preference.CreateDefault(userId);
        Preference preference = await _preferencesRepository.CreateAsync(userPreferenceToCreate, cancellationToken);

        return PreferenceMap.MapToDto(preference);
    }

    public async Task<PreferenceContract> CreatePreferencesInternalAsync(Guid userId,
        CancellationToken cancellationToken)
    {
        Preference preferenceToCreate = Preference.CreateDefault(userId);
        Preference preference = await _preferencesRepository.CreateAsync(preferenceToCreate, cancellationToken);

        return PreferenceMap.MapToContract(preference);
    }

    public async Task UpdatePreferencesAsync(Guid preferenceIdOrUserId, UpdatePreferenceDto preferenceDto,
        CancellationToken cancellationToken)
    {
        Preference dbPreference =
            await _preferenceValidator.ValidateUpdateAsync(preferenceIdOrUserId, preferenceDto, cancellationToken);

        if (dbPreference.Update(GeneralPreferenceMap.MapToModel(preferenceDto.GeneralPreference!),
                FinancePreferenceMap.MapToModel(preferenceDto.FinancePreference!),
                NotificationPreferenceMap.MapToModel(preferenceDto.NotificationPreference!), _messageBroker,
                cancellationToken))
        {
            await _preferencesRepository.UpdateAsync(dbPreference, cancellationToken);
        }
    }
}