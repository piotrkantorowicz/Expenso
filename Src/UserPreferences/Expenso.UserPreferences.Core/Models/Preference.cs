using Expenso.Shared.MessageBroker;
using Expenso.UserPreferences.Core.Mappings;
using Expenso.UserPreferences.Proxy.IntegrationEvents;

namespace Expenso.UserPreferences.Core.Models;

internal sealed record Preference
{
    // ReSharper disable once UnusedMember.Local
    // Required by EF Core
    private Preference() : this(default, default, default, default, default)
    {
    }

    private Preference(Guid preferencesId, Guid userId) : this(preferencesId, userId, GeneralPreference.CreateDefault(),
        FinancePreference.CreateDefault(), NotificationPreference.CreateDefault())
    {
    }

    private Preference(Guid preferencesId, Guid userId, GeneralPreference? generalPreference,
        FinancePreference? financePreference, NotificationPreference? notificationPreference)
    {
        PreferencesId = preferencesId;
        UserId = userId;
        GeneralPreference = generalPreference;
        FinancePreference = financePreference;
        NotificationPreference = notificationPreference;
    }

    public Guid UserId { get; }

    public Guid PreferencesId { get; }

    public GeneralPreference? GeneralPreference { get; private set; }

    public FinancePreference? FinancePreference { get; private set; }

    public NotificationPreference? NotificationPreference { get; private set; }

    public static Preference CreateDefault(Guid userId)
    {
        return new Preference(Guid.NewGuid(), userId);
    }

    public static Preference CreateDefault(Guid preferenceId, Guid userId)
    {
        return new Preference(preferenceId, userId);
    }

    public static Preference Create(Guid preferencesId, Guid userId, GeneralPreference? generalPreference,
        FinancePreference? financePreference, NotificationPreference? notificationPreference)
    {
        return new Preference(preferencesId, userId, generalPreference ?? GeneralPreference.CreateDefault(),
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
                    GeneralPreferenceMap.MapToContract(generalPreference)), cancellationToken);

            isUpdated = true;
        }

        if (FinancePreference != financePreference)
        {
            FinancePreference = financePreference;

            messageBroker.PublishAsync(
                new FinancePreferenceUpdatedIntegrationEvent(UserId,
                    FinancePreferenceMap.MapToContract(financePreference)), cancellationToken);

            isUpdated = true;
        }

        if (NotificationPreference != notificationPreference)
        {
            NotificationPreference = notificationPreference;

            messageBroker.PublishAsync(
                new NotificationPreferenceUpdatedIntegrationEvent(UserId,
                    NotificationPreferenceMap.MapToContract(notificationPreference)), cancellationToken);

            isUpdated = true;
        }

        return isUpdated;
    }
}