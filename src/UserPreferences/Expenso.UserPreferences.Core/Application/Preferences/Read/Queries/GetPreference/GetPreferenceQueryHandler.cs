using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.ExecutionContext;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Maps;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Response;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;

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
        PreferenceQuerySpecification querySpecification = new()
        {
            PreferenceId = query.Payload.PreferenceId,
            PreferenceType = (PreferenceTypes?)query.Payload.PreferenceType,
            UseTracking = false
        };

        Preference preference =
            await _preferencesRepository.GetAsync(preferenceQuerySpecification: querySpecification,
                cancellationToken: cancellationToken) ?? throw new NotFoundException(message: "Preferences not found");

        return GetPreferenceResponseMap.MapTo(preference: preference);
    }
}