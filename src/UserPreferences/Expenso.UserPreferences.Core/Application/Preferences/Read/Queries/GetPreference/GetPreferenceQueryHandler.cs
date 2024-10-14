using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.TypesExtensions;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Maps;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Request;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Response;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;

namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference;

internal sealed class GetPreferenceQueryHandler : IQueryHandler<GetPreferenceQuery, GetPreferenceResponse>
{
    private readonly IPreferencesRepository _preferencesRepository;

    public GetPreferenceQueryHandler(IPreferencesRepository preferencesRepository)
    {
        _preferencesRepository = preferencesRepository ??
                                 throw new ArgumentNullException(paramName: nameof(preferencesRepository));
    }

    public async Task<GetPreferenceResponse?> HandleAsync(GetPreferenceQuery query, CancellationToken cancellationToken)
    {
        PreferenceQuerySpecification querySpecification = new()
        {
            PreferenceId = query.Payload.PreferenceId,
            PreferenceType =
                query.Payload.PreferenceType.SafeCast<PreferenceTypes, GetPreferenceRequest_PreferenceTypes>(),
            UseTracking = false
        };

        Preference preference =
            await _preferencesRepository.GetAsync(preferenceQuerySpecification: querySpecification,
                cancellationToken: cancellationToken) ?? throw new NotFoundException(message: "Preferences not found");

        return GetPreferenceResponseMap.MapTo(preference: preference);
    }
}