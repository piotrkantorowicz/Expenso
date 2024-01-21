using Expenso.Shared.MessageBroker;
using Expenso.UserPreferences.Core.Application.Preferences.Mappings;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.FinancePreferences;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.GeneralPreferences;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.NotificationPreferences;

namespace Expenso.UserPreferences.Core.Domain.Preferences.Model;

internal sealed record Preference
{
    // ReSharper disable once UnusedMember.Local
    // Required by EF Core
    private Preference() : this(PreferenceId.CreateDefault(), UserId.CreateDefault(), default, default, default)
    {
    }

    private Preference(PreferenceId id, UserId userId, GeneralPreference? generalPreference,
        FinancePreference? financePreference, NotificationPreference? notificationPreference)
    {
        Id = id;
        UserId = userId;
        GeneralPreference = generalPreference;
        FinancePreference = financePreference;
        NotificationPreference = notificationPreference;
    }

    public PreferenceId Id { get; }

    public UserId UserId { get; }

    public GeneralPreference? GeneralPreference { get; private set; }

    public FinancePreference? FinancePreference { get; private set; }

    public NotificationPreference? NotificationPreference { get; private set; }

    public static Preference CreateDefault(UserId userId)
    {
        return new Preference(Guid.NewGuid(), userId, GeneralPreference.CreateDefault(),
            FinancePreference.CreateDefault(), NotificationPreference.CreateDefault());
    }

    public static Preference CreateDefault(PreferenceId id, UserId userId)
    {
        return new Preference(id, userId, GeneralPreference.CreateDefault(), FinancePreference.CreateDefault(),
            NotificationPreference.CreateDefault());
    }

    public static Preference Create(PreferenceId id, UserId userId, GeneralPreference? generalPreference,
        FinancePreference? financePreference, NotificationPreference? notificationPreference)
    {
        return new Preference(id, userId, generalPreference ?? GeneralPreference.CreateDefault(),
            financePreference ?? FinancePreference.CreateDefault(),
            notificationPreference ?? NotificationPreference.CreateDefault());
    }

    public bool Update(GeneralPreference generalPreference, FinancePreference financePreference,
        NotificationPreference notificationPreference, IMessageBroker messageBroker,
        CancellationToken cancellationToken)
    {
        bool isUpdated = false;

        if (GeneralPreference != generalPreference)
        {
            GeneralPreference = generalPreference;

            messageBroker.PublishAsync(
                new GeneralPreferenceUpdatedIntegrationEvent(UserId,
                    GeneralPreferenceMap.MapToInternalContract(generalPreference)), cancellationToken);

            isUpdated = true;
        }

        if (FinancePreference != financePreference)
        {
            FinancePreference = financePreference;

            messageBroker.PublishAsync(
                new FinancePreferenceUpdatedIntegrationEvent(UserId,
                    FinancePreferenceMap.MapToInternalContract(financePreference)), cancellationToken);

            isUpdated = true;
        }

        if (NotificationPreference != notificationPreference)
        {
            NotificationPreference = notificationPreference;

            messageBroker.PublishAsync(
                new NotificationPreferenceUpdatedIntegrationEvent(UserId,
                    NotificationPreferenceMap.MapToInternalContract(notificationPreference)), cancellationToken);

            isUpdated = true;
        }

        return isUpdated;
    }
}