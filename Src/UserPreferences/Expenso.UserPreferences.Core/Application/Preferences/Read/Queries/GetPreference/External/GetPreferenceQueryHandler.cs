using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.External.DTO.Maps;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.External;

internal sealed class GetPreferenceQueryHandler(IPreferencesRepository preferencesRepository)
    : IQueryHandler<GetPreferenceQuery, GetPreferenceResponse>
{
    private readonly IPreferencesRepository _preferencesRepository =
        preferencesRepository ?? throw new ArgumentNullException(nameof(preferencesRepository));

    public async Task<GetPreferenceResponse?> HandleAsync(GetPreferenceQuery query,
        CancellationToken cancellationToken = default)
    {
        (Guid? userId, bool? includeFinancePreferences, bool? includeNotificationPreferences,
            bool? includeGeneralPreferences) = query;

        PreferenceFilter filter = new()
        {
            UserId = userId,
            IncludeFinancePreferences = includeFinancePreferences,
            IncludeNotificationPreferences = includeNotificationPreferences,
            IncludeGeneralPreferences = includeGeneralPreferences,
            UseTracking = false
        };

        Preference preference = await _preferencesRepository.GetAsync(filter, cancellationToken) ??
                                throw new NotFoundException($"Preferences for user with id {userId} not found");

        return GetPreferenceResponseMap.MapTo(preference);
    }
}