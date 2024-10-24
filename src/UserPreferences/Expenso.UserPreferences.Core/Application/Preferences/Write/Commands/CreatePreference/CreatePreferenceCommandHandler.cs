using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.DTO.Maps;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Factories;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Response;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;

internal sealed class
    CreatePreferenceCommandHandler : ICommandHandler<CreatePreferenceCommand, CreatePreferenceResponse>
{
    private readonly IPreferencesRepository _preferencesRepository;

    public CreatePreferenceCommandHandler(IPreferencesRepository preferencesRepository)
    {
        _preferencesRepository = preferencesRepository ??
                                 throw new ArgumentNullException(paramName: nameof(preferencesRepository));
    }

    public async Task<CreatePreferenceResponse> HandleAsync(CreatePreferenceCommand command,
        CancellationToken cancellationToken)
    {
        Guid userId = command.Payload!.UserId;

        PreferenceQuerySpecification querySpecification = new()
        {
            UserId = userId,
            UseTracking = false
        };

        bool dbUserPreferencesExists =
            await _preferencesRepository.ExistsAsync(preferenceQuerySpecification: querySpecification,
                cancellationToken: cancellationToken);

        if (dbUserPreferencesExists)
        {
            throw new ConflictException(message: $"Preferences for user with ID {userId} already exists");
        }

        Preference preferenceToCreate = PreferenceFactory.Create(userId: userId);

        Preference preference =
            await _preferencesRepository.CreateAsync(preference: preferenceToCreate,
                cancellationToken: cancellationToken);

        return CreatePreferenceResponseMap.MapTo(preference: preference);
    }
}