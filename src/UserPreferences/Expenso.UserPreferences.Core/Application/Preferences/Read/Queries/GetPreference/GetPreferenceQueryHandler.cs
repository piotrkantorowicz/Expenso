using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.ExecutionContext;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Maps;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference;

internal sealed class GetPreferenceQueryHandler : IQueryHandler<GetPreferenceQuery, GetPreferenceResponse>
{
    private readonly IExecutionContextAccessor _executionContextAccessor;
    private readonly IPreferencesRepository _preferencesRepository;

    public GetPreferenceQueryHandler(IPreferencesRepository preferencesRepository,
        IExecutionContextAccessor executionContextAccessor)
    {
        _executionContextAccessor = executionContextAccessor ??
                                    throw new ArgumentNullException(paramName: nameof(executionContextAccessor));

        _preferencesRepository = preferencesRepository ??
                                 throw new ArgumentNullException(paramName: nameof(preferencesRepository));
    }

    public async Task<GetPreferenceResponse?> HandleAsync(GetPreferenceQuery query, CancellationToken cancellationToken)
    {
        PreferenceFilter filter = GetFilter(query: query);

        Preference preference =
            await _preferencesRepository.GetAsync(preferenceFilter: filter, cancellationToken: cancellationToken) ??
            throw new NotFoundException(message: "Preferences not found");

        return GetPreferenceResponseMap.MapTo(preference: preference);
    }

    private PreferenceFilter GetFilter(GetPreferenceQuery query)
    {
        (_, Guid? preferenceIdOrUserId, bool? forCurrentUser, bool? includeFinancePreferences,
            bool? includeNotificationPreferences, bool? includeGeneralPreferences) = query;

        if (forCurrentUser is true)
        {
            preferenceIdOrUserId = Guid.TryParse(input: _executionContextAccessor.Get()?.UserContext?.UserId,
                result: out Guid id)
                ? id
                : Guid.Empty;
        }

        return new PreferenceFilter
        {
            PreferenceIdOrUserId = preferenceIdOrUserId,
            IncludeFinancePreferences = includeFinancePreferences,
            IncludeNotificationPreferences = includeNotificationPreferences,
            IncludeGeneralPreferences = includeGeneralPreferences,
            UseTracking = false
        };
    }
}