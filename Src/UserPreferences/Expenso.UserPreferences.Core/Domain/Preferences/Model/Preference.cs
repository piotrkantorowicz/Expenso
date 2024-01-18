using Expenso.Shared.MessageBroker;
using Expenso.UserPreferences.Core.Application.Mappings;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;
using Expenso.UserPreferences.Proxy.IntegrationEvents;

namespace Expenso.UserPreferences.Core.Domain.Preferences.Model;

internal sealed record Preference
{
    // ReSharper disable once UnusedMember.Local
    // Required by EF Core
    private Preference() : this(PreferenceId.CreateDefault(), UserId.CreateDefault(), default, default, default)
    {
    }

    private Preference(PreferenceId preferenceId, UserId userId, GeneralPreference? generalPreference,
        FinancePreference? financePreference, NotificationPreference? notificationPreference)
    {
        PreferenceId = preferenceId;
        UserId = userId;
        GeneralPreference = generalPreference;
        FinancePreference = financePreference;
        NotificationPreference = notificationPreference;
    }

    public PreferenceId PreferenceId { get; }

    public UserId UserId { get; }

    public GeneralPreference? GeneralPreference { get; private set; }

    public FinancePreference? FinancePreference { get; private set; }

    public NotificationPreference? NotificationPreference { get; private set; }

    public static Preference CreateDefault(UserId userId)
    {
        return new Preference(Guid.NewGuid(), userId, GeneralPreference.CreateDefault(),
            FinancePreference.CreateDefault(), NotificationPreference.CreateDefault());
    }

    public static Preference CreateDefault(PreferenceId preferenceId, UserId userId)
    {
        return new Preference(preferenceId, userId, GeneralPreference.CreateDefault(),
            FinancePreference.CreateDefault(), NotificationPreference.CreateDefault());
    }

    public static Preference Create(PreferenceId preferencesId, UserId userId, GeneralPreference? generalPreference,
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

            messageBroker.PublishAsync(new GeneralPreferenceUpdatedIntegrationEvent(UserId!,
                    GeneralPreferenceMap.MapToContract(generalPreference)), cancellationToken);

            isUpdated = true;
        }

        if (FinancePreference != financePreference)
        {
            FinancePreference = financePreference;

            messageBroker.PublishAsync(new FinancePreferenceUpdatedIntegrationEvent(UserId!,
                    FinancePreferenceMap.MapToContract(financePreference)), cancellationToken);

            isUpdated = true;
        }

        if (NotificationPreference != notificationPreference)
        {
            NotificationPreference = notificationPreference;

            messageBroker.PublishAsync(new NotificationPreferenceUpdatedIntegrationEvent(UserId!,
                    NotificationPreferenceMap.MapToContract(notificationPreference)), cancellationToken);

            isUpdated = true;
        }

        return isUpdated;
    }
}