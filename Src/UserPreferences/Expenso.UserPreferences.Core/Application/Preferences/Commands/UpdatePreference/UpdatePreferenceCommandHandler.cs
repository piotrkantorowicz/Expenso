using Expenso.Shared.Commands;
using Expenso.Shared.MessageBroker;
using Expenso.Shared.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.UpdatePreferences.Request;
using Expenso.UserPreferences.Core.Application.Preferences.Mappings;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;

namespace Expenso.UserPreferences.Core.Application.Preferences.Commands.UpdatePreference;

internal sealed class UpdatePreferenceCommandHandler(
    IPreferencesRepository preferencesRepository,
    IMessageBroker messageBroker) : ICommandHandler<UpdatePreferenceCommand>
{
    private readonly IMessageBroker _messageBroker =
        messageBroker ?? throw new ArgumentNullException(nameof(messageBroker));

    private readonly IPreferencesRepository _preferencesRepository =
        preferencesRepository ?? throw new ArgumentNullException(nameof(preferencesRepository));

    public async Task HandleAsync(UpdatePreferenceCommand command, CancellationToken cancellationToken = default)
    {
        Preference? dbPreference =
            await _preferencesRepository.GetByIdAsync(PreferenceId.Create(command.PreferenceOrUserId), true,
                cancellationToken) ??
            await _preferencesRepository.GetByUserIdAsync(UserId.Create(command.PreferenceOrUserId), true,
                cancellationToken);

        if (dbPreference is null)
        {
            throw new ConflictException(
                $"User preferences for user with id {command.PreferenceOrUserId} or with own id: {command.PreferenceOrUserId} haven't been found.");
        }

        UpdatePreferenceRequest preference = command.Preference!;

        if (dbPreference.Update(GeneralPreferenceMap.MapToModel(preference.GeneralPreference!),
                FinancePreferenceMap.MapToModel(preference.FinancePreference!),
                NotificationPreferenceMap.MapToModel(preference.NotificationPreference!), _messageBroker,
                cancellationToken))
        {
            await _preferencesRepository.UpdateAsync(dbPreference, cancellationToken);
        }
    }
}