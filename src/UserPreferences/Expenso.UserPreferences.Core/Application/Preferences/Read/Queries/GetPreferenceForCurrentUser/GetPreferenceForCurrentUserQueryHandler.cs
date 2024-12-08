using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;
using Expenso.Shared.System.Types.ExecutionContext;
using Expenso.Shared.System.Types.TypesExtensions;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser.DTO.Maps;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser.DTO.Request;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser.DTO.Response;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Specifications;

namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser;

internal sealed class
    GetPreferenceForCurrentUserQueryHandler : IQueryHandler<GetPreferenceForCurrentUserQuery,
    GetPreferenceForCurrentUserResponse>
{
    private readonly IExecutionContextAccessor _executionContextAccessor;
    private readonly IPreferencesRepository _preferencesRepository;

    public GetPreferenceForCurrentUserQueryHandler(IPreferencesRepository preferencesRepository,
        IExecutionContextAccessor executionContextAccessor)
    {
        _executionContextAccessor = executionContextAccessor ??
                                    throw new ArgumentNullException(paramName: nameof(executionContextAccessor));

        _preferencesRepository = preferencesRepository ??
                                 throw new ArgumentNullException(paramName: nameof(preferencesRepository));
    }

    public async Task<GetPreferenceForCurrentUserResponse?> HandleAsync(GetPreferenceForCurrentUserQuery query,
        CancellationToken cancellationToken)
    {
        if (Guid.TryParse(input: _executionContextAccessor.Get()?.UserContext?.UserId,
                result: out Guid currentUserId) is false)
        {
            throw new NotFoundException(
                message:
                $"Unable to retrieve preference due to invalid user ID format: {_executionContextAccessor.Get()?.UserContext?.UserId}");
        }

        PreferenceQuerySpecification querySpecification = new()
        {
            UserId = currentUserId,
            Includes = query.Payload?.Includes
                .SafeCast<PreferenceIncludes, GetPreferenceForCurrentUserRequestPreferenceIncludes>(),
            UseTracking = false
        };

        Preference preference = await _preferencesRepository.GetAsync(querySpecification: querySpecification,
                cancellationToken: cancellationToken) ?? throw new NotFoundException(resourceName: nameof(Preference),
                identifierType: IdentifierType.Query(), identifier: querySpecification);

        return GetPreferenceForCurrentUserResponseMap.MapTo(preference: preference);
    }
}