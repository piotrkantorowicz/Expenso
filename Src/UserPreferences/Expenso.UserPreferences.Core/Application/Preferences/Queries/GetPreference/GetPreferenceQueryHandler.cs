using Expenso.Shared.Queries;
using Expenso.Shared.Types.Exceptions;
using Expenso.Shared.UserContext;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.GetPreferences.Response;
using Expenso.UserPreferences.Core.Application.Preferences.Mappings;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;

namespace Expenso.UserPreferences.Core.Application.Preferences.Queries.GetPreference;

internal sealed class GetPreferenceQueryHandler(
    IPreferencesRepository preferencesRepository,
    IUserContextAccessor userContextAccessor) : IQueryHandler<GetPreferenceQuery, GetPreferenceResponse>
{
    private readonly IPreferencesRepository _preferencesRepository =
        preferencesRepository ?? throw new ArgumentNullException(nameof(preferencesRepository));

    private readonly IUserContextAccessor _userContextAccessor =
        userContextAccessor ?? throw new ArgumentNullException(nameof(userContextAccessor));

    public async Task<GetPreferenceResponse?> HandleAsync(GetPreferenceQuery query,
        CancellationToken cancellationToken = default)
    {
        (Guid? preferenceId, Guid? userId) = query;

        if (preferenceId.HasValue)
        {
            return await GetPreferencesAsync(preferenceId.Value, cancellationToken);
        }

        if (userId.HasValue)
        {
            return await GetPreferencesForUserAsync(userId.Value, cancellationToken);
        }

        return await GetPreferencesForCurrentUserAsync(cancellationToken);
    }

    private async Task<GetPreferenceResponse> GetPreferencesAsync(Guid preferenceId,
        CancellationToken cancellationToken)
    {
        Preference preference = await _preferencesRepository.GetByIdAsync(preferenceId, false, cancellationToken) ??
                                throw new NotFoundException($"Preferences with id {preferenceId} not found.");

        return PreferenceMap.MapToGetResponse(preference);
    }

    private Task<GetPreferenceResponse> GetPreferencesForCurrentUserAsync(CancellationToken cancellationToken)
    {
        Guid userId = Guid.TryParse(_userContextAccessor.Get()?.UserId, out Guid id) ? id : Guid.Empty;

        if (userId == Guid.Empty)
        {
            throw new NotFoundException(
                "Preferences for current user not found, because user id from user context is empty.");
        }

        return GetPreferencesForUserAsync(userId, cancellationToken);
    }

    private async Task<GetPreferenceResponse> GetPreferencesForUserAsync(Guid userId,
        CancellationToken cancellationToken)
    {
        Preference preference = await _preferencesRepository.GetByUserIdAsync(userId, false, cancellationToken) ??
                                throw new NotFoundException($"Preferences for user with id {userId} not found.");

        return PreferenceMap.MapToGetResponse(preference);
    }
}