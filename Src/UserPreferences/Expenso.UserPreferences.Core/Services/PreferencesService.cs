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
    PreferenceValidator preferenceValidator) : IPreferencesService
{
    private readonly IMessageBroker _messageBroker =
        messageBroker ?? throw new ArgumentNullException(nameof(messageBroker));

    private readonly IPreferencesRepository _preferencesRepository =
        preferencesRepository ?? throw new ArgumentNullException(nameof(preferencesRepository));

    private readonly PreferenceValidator _preferenceValidator =
        preferenceValidator ?? throw new ArgumentNullException(nameof(preferenceValidator));

    private readonly IUserContextAccessor _userContextAccessor =
        userContextAccessor ?? throw new ArgumentNullException(nameof(userContextAccessor));

    public Task<PreferenceDto> GetPreferencesForCurrentUserAsync(CancellationToken cancellationToken)
    {
        Guid userId = Guid.TryParse(_userContextAccessor.Get()?.UserId, out Guid id) ? id : Guid.Empty;

        return GetPreferencesAsync(userId, cancellationToken);
    }

    public async Task<PreferenceDto> GetPreferencesAsync(Guid userId, CancellationToken cancellationToken)
    {
        Preference? userPreferences = await _preferencesRepository.GetByUserIdAsync(userId, false, cancellationToken);

        if (userPreferences is null)
        {
            throw new NotFoundException($"User preferences for user with id {userId} not found.");
        }

        return PreferenceMap.MapToDto(userPreferences);
    }

    public async Task<PreferenceContract> GetPreferencesInternalAsync(Guid userId, CancellationToken cancellationToken)
    {
        Preference? userPreferences = await _preferencesRepository.GetByUserIdAsync(userId, false, cancellationToken);

        if (userPreferences is null)
        {
            throw new NotFoundException($"User preferences for user with id {userId} not found.");
        }

        return PreferenceMap.MapToContract(userPreferences);
    }

    public async Task CreatePreferencesAsync(Guid userId, CancellationToken cancellationToken)
    {
        await _preferenceValidator.ValidateCreateAsync(userId, cancellationToken);
        Preference userPreferenceToCreate = Preference.CreateDefault(userId);
        await _preferencesRepository.CreateAsync(userPreferenceToCreate, cancellationToken);
    }

    public async Task CreatePreferencesInternalAsync(Guid userId, CancellationToken cancellationToken)
    {
        Preference userPreferenceToCreate = Preference.CreateDefault(userId);
        await _preferencesRepository.CreateAsync(userPreferenceToCreate, cancellationToken);
    }

    public async Task UpdatePreferencesAsync(Guid preferenceIdOrUserId, UpdatePreferenceDto preferenceDto,
        CancellationToken cancellationToken)
    {
        Preference dbUserPreference =
            await _preferenceValidator.ValidateUpdateAsync(preferenceIdOrUserId, preferenceDto, cancellationToken);

        if (dbUserPreference.Update(GeneralPreferenceMap.MapToModel(preferenceDto.GeneralPreference!),
                FinancePreferenceMap.MapToModel(preferenceDto.FinancePreference!),
                NotificationPreferenceMap.MapToModel(preferenceDto.NotificationPreference!), _messageBroker,
                cancellationToken))
        {
            await _preferencesRepository.UpdateAsync(dbUserPreference, cancellationToken);
        }
    }
}