using Expenso.Shared.Queries;
using Expenso.Shared.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.Mappings;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

namespace Expenso.UserPreferences.Core.Application.Preferences.Internal.Queries.GetPreference;

internal sealed class GetPreferenceInternalQueryHandler(IPreferencesRepository preferencesRepository)
    : IQueryHandler<GetPreferenceInternalQuery, GetPreferenceInternalResponse>
{
    private readonly IPreferencesRepository _preferencesRepository =
        preferencesRepository ?? throw new ArgumentNullException(nameof(preferencesRepository));

    public async Task<GetPreferenceInternalResponse?> HandleAsync(GetPreferenceInternalQuery query,
        CancellationToken cancellationToken = default)
    {
        return await GetPreferencesForUserAsync(query.UserId, cancellationToken);
    }

    private async Task<GetPreferenceInternalResponse> GetPreferencesForUserAsync(Guid userId,
        CancellationToken cancellationToken)
    {
        Preference preference = await _preferencesRepository.GetByUserIdAsync(userId, false, cancellationToken) ??
                                throw new NotFoundException($"Preferences for user with id {userId} not found.");

        return PreferenceMap.MapToInternalGetRequest(preference);
    }
}