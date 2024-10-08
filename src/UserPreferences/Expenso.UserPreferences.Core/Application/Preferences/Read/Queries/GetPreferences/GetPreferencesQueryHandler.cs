using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.ExecutionContext;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferences.DTO.Maps;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferences;

internal sealed class GetPreferencesQueryHandler : IQueryHandler<GetPreferencesQuery, GetPreferencesResponse>
{
    private readonly IExecutionContextAccessor _executionContextAccessor;
    private readonly IPreferencesRepository _preferencesRepository;

    public GetPreferencesQueryHandler(IPreferencesRepository preferencesRepository,
        IExecutionContextAccessor executionContextAccessor)
    {
        _executionContextAccessor = executionContextAccessor ??
                                    throw new ArgumentNullException(paramName: nameof(executionContextAccessor));

        _preferencesRepository = preferencesRepository ??
                                 throw new ArgumentNullException(paramName: nameof(preferencesRepository));
    }

    public async Task<GetPreferencesResponse?> HandleAsync(GetPreferencesQuery query,
        CancellationToken cancellationToken)
    {
        PreferenceQuerySpecification querySpecification = new()
        {
            PreferenceId = query.Payload.PreferenceId,
            UserId = query.Payload.UserId,
            PreferenceType = (PreferenceTypes?)query.Payload.PreferenceType,
            UseTracking = false
        };

        Preference preference = await _preferencesRepository.GetAsync(preferenceQuerySpecification: querySpecification,
                                    cancellationToken: cancellationToken) ??
                                throw new NotFoundException(message: "Preferences not found");

        return GetPreferencesResponseMap.MapTo(preference: preference);
    }
}