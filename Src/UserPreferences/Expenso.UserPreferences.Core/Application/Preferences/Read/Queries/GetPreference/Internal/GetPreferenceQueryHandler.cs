using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.UserContext;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.Internal.DTO.Maps;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.Internal.DTO.Response;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;

namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.Internal;

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
        PreferenceFilter filter = GetFilter(query);

        Preference preference = await _preferencesRepository.GetAsync(filter, cancellationToken) ??
                                throw new NotFoundException("Preferences not found");

        return GetPreferenceResponseMap.MapTo(preference);
    }

    private PreferenceFilter GetFilter(GetPreferenceQuery query)
    {
        (Guid? preferenceId, Guid? userId, bool? forCurrentUser, bool? includeFinancePreferences,
            bool? includeNotificationPreferences, bool? includeGeneralPreferences) = query;

        if (forCurrentUser == true)
        {
            userId = Guid.TryParse(_userContextAccessor.Get()?.UserId, out Guid id) ? id : Guid.Empty;
        }

        return new PreferenceFilter
        {
            Id = preferenceId,
            UserId = userId,
            IncludeFinancePreferences = includeFinancePreferences,
            IncludeNotificationPreferences = includeNotificationPreferences,
            IncludeGeneralPreferences = includeGeneralPreferences,
            UseTracking = false
        };
    }
}