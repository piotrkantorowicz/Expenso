using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;
using Expenso.Shared.System.Types.TypesExtensions;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferences.DTO.Maps;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Specifications;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Request;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferences;

internal sealed class GetPreferencesQueryHandler : IQueryHandler<GetPreferencesQuery, GetPreferencesResponse>
{
    private readonly IPreferencesRepository _preferencesRepository;

    public GetPreferencesQueryHandler(IPreferencesRepository preferencesRepository)
    {
        _preferencesRepository = preferencesRepository ??
                                 throw new ArgumentNullException(paramName: nameof(preferencesRepository));
    }

    public async Task<GetPreferencesResponse?> HandleAsync(GetPreferencesQuery query,
        CancellationToken cancellationToken)
    {
        PreferenceQuerySpecification querySpecification = new()
        {
            PreferenceId = query.Payload?.PreferenceId,
            UserId = query.Payload?.UserId,
            Includes = query.Payload?.Includes.SafeCast<PreferenceIncludes, GetPreferencesRequestPreferenceIncludes>(),
            UseTracking = false
        };

        Preference preference = await _preferencesRepository.GetAsync(querySpecification: querySpecification,
                cancellationToken: cancellationToken) ?? throw new NotFoundException(resourceName: nameof(Preference),
                identifierType: IdentifierType.Query(), identifier: querySpecification);

        return GetPreferencesResponseMap.MapTo(preference: preference);
    }
}