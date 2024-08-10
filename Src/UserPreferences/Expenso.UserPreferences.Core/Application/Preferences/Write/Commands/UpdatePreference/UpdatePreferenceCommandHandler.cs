using Expenso.Shared.Commands;
using Expenso.Shared.Integration.MessageBroker;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Maps;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.UpdatePreference.FinancePreferences;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.UpdatePreference.GeneralPreferences;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.UpdatePreference.NotificationPreferences;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;

internal sealed class UpdatePreferenceCommandHandler(
    IPreferencesRepository preferencesRepository,
    IMessageBroker messageBroker,
    IMessageContextFactory messageContextFactory) : ICommandHandler<UpdatePreferenceCommand>
{
    private readonly IMessageBroker _messageBroker =
        messageBroker ?? throw new ArgumentNullException(paramName: nameof(messageBroker));

    private readonly IMessageContextFactory _messageContextFactory = messageContextFactory ??
                                                                     throw new ArgumentNullException(
                                                                         paramName: nameof(messageContextFactory));

    private readonly IPreferencesRepository _preferencesRepository = preferencesRepository ??
                                                                     throw new ArgumentNullException(
                                                                         paramName: nameof(preferencesRepository));

    public async Task HandleAsync(UpdatePreferenceCommand command, CancellationToken cancellationToken)
    {
        (_, Guid preferenceOrUserId, UpdatePreferenceRequest? updatePreferenceRequest) = command;

        (UpdatePreferenceRequest_FinancePreference? financePreferenceRequest,
            UpdatePreferenceRequest_NotificationPreference? notificationPreferenceRequest,
            UpdatePreferenceRequest_GeneralPreference? generalPreferenceRequest) = updatePreferenceRequest!;

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

        Preference? dbPreference =
            await _preferencesRepository.GetAsync(preferenceFilter: preferenceIdFilter,
                cancellationToken: cancellationToken) ??
            await _preferencesRepository.GetAsync(preferenceFilter: userIdFilter, cancellationToken: cancellationToken);

        if (dbPreference is null)
        {
            throw new ConflictException(
                message:
                $"User preferences for user with id {preferenceOrUserId} or with own id: {preferenceOrUserId} haven't been found.");
        }

        IEnumerable<Task> integrationMessagesTasks = Update(preference: dbPreference,
            updateGeneralPreference: generalPreferenceRequest!, updateFinancePreference: financePreferenceRequest!,
            updateNotificationPreference: notificationPreferenceRequest!, cancellationToken: cancellationToken);

        await _preferencesRepository.UpdateAsync(preference: dbPreference, cancellationToken: cancellationToken);
        await Task.WhenAll(tasks: integrationMessagesTasks);
    }

    private IEnumerable<Task> Update(Preference preference,
        UpdatePreferenceRequest_GeneralPreference updateGeneralPreference,
        UpdatePreferenceRequest_FinancePreference updateFinancePreference,
        UpdatePreferenceRequest_NotificationPreference updateNotificationPreference,
        CancellationToken cancellationToken)
    {
        GeneralPreference generalPreference =
            UpdatePreferenceRequestMap.MapFrom(generalGeneralPreference: updateGeneralPreference);

        FinancePreference financePreference =
            UpdatePreferenceRequestMap.MapFrom(financePreference: updateFinancePreference);

        NotificationPreference notificationPreference =
            UpdatePreferenceRequestMap.MapFrom(updatePreferenceRequest: updateNotificationPreference);

        ICollection<Task> tasks = [];

        if (preference.GeneralPreference is not null && preference.GeneralPreference != generalPreference)
        {
            preference.GeneralPreference = preference.GeneralPreference with
            {
                UseDarkMode = generalPreference.UseDarkMode
            };

            tasks.Add(item: _messageBroker.PublishAsync(
                @event: new GeneralPreferenceUpdatedIntegrationEvent(MessageContext: _messageContextFactory.Current(),
                    UserId: preference.UserId,
                    GeneralPreference: UpdatePreferenceContractMap.MapTo(generalPreference: generalPreference)),
                cancellationToken: cancellationToken));
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

            tasks.Add(item: _messageBroker.PublishAsync(
                @event: new FinancePreferenceUpdatedIntegrationEvent(MessageContext: _messageContextFactory.Current(),
                    UserId: preference.UserId,
                    FinancePreference: UpdatePreferenceContractMap.MapTo(financePreference: financePreference)),
                cancellationToken: cancellationToken));
        }

        if (preference.NotificationPreference is not null &&
            preference.NotificationPreference != notificationPreference)
        {
            preference.NotificationPreference = preference.NotificationPreference with
            {
                SendFinanceReportEnabled = notificationPreference.SendFinanceReportEnabled,
                SendFinanceReportInterval = notificationPreference.SendFinanceReportInterval
            };

            tasks.Add(item: _messageBroker.PublishAsync(
                @event: new NotificationPreferenceUpdatedIntegrationEvent(
                    MessageContext: _messageContextFactory.Current(), UserId: preference.UserId,
                    NotificationPreference: UpdatePreferenceContractMap.MapTo(
                        notificationPreference: notificationPreference)), cancellationToken: cancellationToken));
        }

        return tasks;
    }
}