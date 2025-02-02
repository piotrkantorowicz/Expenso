using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.DTO.Maps;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Factories;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Specifications;
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
        PreferenceQuerySpecification querySpecification = new(UserId: command.Payload?.UserId, UseTracking: false);

        bool dbUserPreferencesExists = await _preferencesRepository.ExistsAsync(querySpecification: querySpecification,
            cancellationToken: cancellationToken);

        if (dbUserPreferencesExists)
        {
            throw ConflictException.AlreadyExists(resourceName: nameof(Preference),
                identifierType: IdentifierType.Query(), identifier: querySpecification);
        }

        if (command.Payload?.PreferenceId is not null)
        {
            querySpecification =
                new PreferenceQuerySpecification(PreferenceId: command.Payload.PreferenceId, UseTracking: false);

            bool dbPreferenceExists = await _preferencesRepository.ExistsAsync(querySpecification: querySpecification,
                cancellationToken: cancellationToken);

            if (dbPreferenceExists)
            {
                throw ConflictException.AlreadyExists(resourceName: nameof(Preference),
                    identifierType: IdentifierType.Query(), identifier: querySpecification);
            }
        }

        Preference preferenceToCreate = PreferenceFactory.Create(preferenceId: command.Payload?.PreferenceId,
            userId: command.Payload!.UserId);

        Preference preference =
            await _preferencesRepository.CreateAsync(preference: preferenceToCreate,
                cancellationToken: cancellationToken);

        return CreatePreferenceResponseMap.MapTo(preference: preference);
    }
}