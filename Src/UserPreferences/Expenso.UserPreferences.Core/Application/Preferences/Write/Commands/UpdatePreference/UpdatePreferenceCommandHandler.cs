using Expenso.Shared.Commands;
using Expenso.Shared.Integration.MessageBroker;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Maps;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.FinancePreferences;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.GeneralPreferences;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.NotificationPreferences;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;

internal sealed class UpdatePreferenceCommandHandler(
    IPreferencesRepository preferencesRepository,
    IMessageBroker messageBroker,
    IMessageContextFactory messageContextFactory) : ICommandHandler<UpdatePreferenceCommand>
{
    private readonly IMessageBroker _messageBroker =
        messageBroker ?? throw new ArgumentNullException(nameof(messageBroker));

    private readonly IMessageContextFactory _messageContextFactory =
        messageContextFactory ?? throw new ArgumentNullException(nameof(messageContextFactory));

    private readonly IPreferencesRepository _preferencesRepository =
        preferencesRepository ?? throw new ArgumentNullException(nameof(preferencesRepository));

    public async Task HandleAsync(UpdatePreferenceCommand command, CancellationToken cancellationToken = default)
    {
        (IMessageContext messageContext, Guid preferenceOrUserId, UpdatePreferenceRequest? updatePreferenceRequest) =
            command;

        (UpdateFinancePreferenceRequest? financePreferenceRequest,
            UpdateNotificationPreferenceRequest? notificationPreferenceRequest,
            UpdateGeneralPreferenceRequest? generalPreferenceRequest) = updatePreferenceRequest!;

        PreferenceFilter preferenceFilter = new(UseTracking: true, IncludeFinancePreferences: true,
            IncludeGeneralPreferences: true, IncludeNotificationPreferences: true);

        PreferenceFilter preferenceIdFilter = preferenceFilter with
        {
            Id = preferenceOrUserId
        };

        PreferenceFilter userIdFilter = preferenceFilter with
        {
            UserId = preferenceOrUserId
        };

        Preference? dbPreference = await _preferencesRepository.GetAsync(preferenceIdFilter, cancellationToken) ??
                                   await _preferencesRepository.GetAsync(userIdFilter, cancellationToken);

        if (dbPreference is null)
        {
            throw new ConflictException(
                $"User preferences for user with id {preferenceOrUserId} or with own id: {preferenceOrUserId} haven't been found");
        }

        IEnumerable<Task> integrationMessagesTasks = Update(dbPreference, generalPreferenceRequest!,
            financePreferenceRequest!, notificationPreferenceRequest!, cancellationToken);

        await _preferencesRepository.UpdateAsync(dbPreference, cancellationToken);
        await Task.WhenAll(integrationMessagesTasks);
    }

    private IEnumerable<Task> Update(Preference preference, UpdateGeneralPreferenceRequest updateGeneralPreference,
        UpdateFinancePreferenceRequest updateFinancePreference,
        UpdateNotificationPreferenceRequest updateNotificationPreference, CancellationToken cancellationToken)
    {
        GeneralPreference generalPreference = UpdatePreferenceRequestMap.MapFrom(updateGeneralPreference);
        FinancePreference financePreference = UpdatePreferenceRequestMap.MapFrom(updateFinancePreference);

        NotificationPreference notificationPreference =
            UpdatePreferenceRequestMap.MapFrom(updateNotificationPreference);

        ICollection<Task> tasks = [];

        if (preference.GeneralPreference is not null && preference.GeneralPreference != generalPreference)
        {
            preference.GeneralPreference = preference.GeneralPreference with
            {
                UseDarkMode = generalPreference.UseDarkMode
            };

            tasks.Add(_messageBroker.PublishAsync(new GeneralPreferenceUpdatedIntegrationEvent(
                _messageContextFactory.Current(), preference.UserId,
                    UpdatePreferenceInternalContractMap.MapTo(generalPreference)), cancellationToken));
        }

        if (preference.FinancePreference is not null && preference.FinancePreference != financePreference)
        {
            preference.FinancePreference = preference.FinancePreference with
            {
                AllowAddFinancePlanSubOwners = financePreference.AllowAddFinancePlanSubOwners,
                MaxNumberOfSubFinancePlanSubOwners = financePreference.MaxNumberOfSubFinancePlanSubOwners,
                AllowAddFinancePlanReviewers = financePreference.AllowAddFinancePlanReviewers,
                MaxNumberOfFinancePlanReviewers = financePreference.MaxNumberOfFinancePlanReviewers
            };

            tasks.Add(_messageBroker.PublishAsync(new FinancePreferenceUpdatedIntegrationEvent(
                _messageContextFactory.Current(), preference.UserId,
                    UpdatePreferenceInternalContractMap.MapTo(financePreference)), cancellationToken));
        }

        if (preference.NotificationPreference is not null &&
            preference.NotificationPreference != notificationPreference)
        {
            preference.NotificationPreference = preference.NotificationPreference with
            {
                SendFinanceReportEnabled = notificationPreference.SendFinanceReportEnabled,
                SendFinanceReportInterval = notificationPreference.SendFinanceReportInterval
            };

            tasks.Add(_messageBroker.PublishAsync(new NotificationPreferenceUpdatedIntegrationEvent(
                _messageContextFactory.Current(), preference.UserId,
                    UpdatePreferenceInternalContractMap.MapTo(notificationPreference)), cancellationToken));
        }

        return tasks;
    }
}