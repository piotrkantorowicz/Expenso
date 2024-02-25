using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.ExecutionContext;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Maps;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference;

internal sealed class GetPreferenceQueryHandler(
    IPreferencesRepository preferencesRepository,
    IExecutionContextAccessor executionContextAccessor) : IQueryHandler<GetPreferenceQuery, GetPreferenceResponse>
{
    private readonly IExecutionContextAccessor _executionContextAccessor =
        executionContextAccessor ?? throw new ArgumentNullException(nameof(executionContextAccessor));

    private readonly IPreferencesRepository _preferencesRepository =
        preferencesRepository ?? throw new ArgumentNullException(nameof(preferencesRepository));

    public async Task<GetPreferenceResponse?> HandleAsync(GetPreferenceQuery query, CancellationToken cancellationToken)
    {
        PreferenceFilter filter = GetFilter(query);

        Preference preference = await _preferencesRepository.GetAsync(filter, cancellationToken) ??
                                throw new NotFoundException("Preferences not found.");

        return GetPreferenceResponseMap.MapTo(preference);
    }

    private PreferenceFilter GetFilter(GetPreferenceQuery query)
    {
        (_, Guid? preferenceId, Guid? userId, bool? forCurrentUser, bool? includeFinancePreferences,
            bool? includeNotificationPreferences, bool? includeGeneralPreferences) = query;

        if (forCurrentUser == true)
        {
            userId = Guid.TryParse(_executionContextAccessor.Get()?.UserContext?.UserId, out Guid id) ? id : Guid.Empty;
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