using Expenso.Shared.Commands;
using Expenso.Shared.MessageBroker;
using Expenso.Shared.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.UpdatePreferences.Request;
using Expenso.UserPreferences.Core.Application.Preferences.Mappings;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.FinancePreferences;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.GeneralPreferences;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.NotificationPreferences;

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
        (Guid preferenceOrUserId, UpdatePreferenceRequest? updatePreferenceRequest) = command;

        (UpdateFinancePreferenceRequest? financePreferenceRequest,
            UpdateNotificationPreferenceRequest? notificationPreferenceRequest,
            UpdateGeneralPreferenceRequest? generalPreferenceRequest) = updatePreferenceRequest!;

        Preference? dbPreference =
            await _preferencesRepository.GetByIdAsync(PreferenceId.Create(preferenceOrUserId), true,
                cancellationToken) ??
            await _preferencesRepository.GetByUserIdAsync(UserId.Create(preferenceOrUserId), true, cancellationToken);

        if (dbPreference is null)
        {
            throw new ConflictException(
                $"User preferences for user with id {preferenceOrUserId} or with own id: {preferenceOrUserId} haven't been found.");
        }

        GeneralPreference generalPreference = GeneralPreferenceMap.MapToModel(generalPreferenceRequest!);
        FinancePreference financePreference = FinancePreferenceMap.MapToModel(financePreferenceRequest!);

        NotificationPreference notificationPreference =
            NotificationPreferenceMap.MapToModel(notificationPreferenceRequest!);

        PreferenceChangeType preferenceChangeType =
            dbPreference.Update(generalPreference, financePreference, notificationPreference);

        if (preferenceChangeType.IsAnyPreferencesChanged)
        {
            List<Task> tasks = [];

            if (preferenceChangeType.GeneralPreferencesChanged)
            {
                tasks.Add(_messageBroker.PublishAsync(
                    new GeneralPreferenceUpdatedIntegrationEvent(dbPreference.UserId,
                        GeneralPreferenceMap.MapToInternalContract(generalPreference)), cancellationToken));
            }

            if (preferenceChangeType.FinancePreferencesChanged)
            {
                tasks.Add(_messageBroker.PublishAsync(
                    new FinancePreferenceUpdatedIntegrationEvent(dbPreference.UserId,
                        FinancePreferenceMap.MapToInternalContract(financePreference)), cancellationToken));
            }

            if (preferenceChangeType.NotificationPreferencesChanged)
            {
                tasks.Add(_messageBroker.PublishAsync(
                    new NotificationPreferenceUpdatedIntegrationEvent(dbPreference.UserId,
                        NotificationPreferenceMap.MapToInternalContract(notificationPreference)), cancellationToken));
            }

            await _preferencesRepository.UpdateAsync(dbPreference, cancellationToken);
            await Task.WhenAll(tasks);
        }
    }
}