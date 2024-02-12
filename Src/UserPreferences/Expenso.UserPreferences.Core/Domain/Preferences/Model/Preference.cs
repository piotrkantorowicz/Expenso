using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

namespace Expenso.UserPreferences.Core.Domain.Preferences.Model;

internal sealed record Preference
{
    // ReSharper disable once UnusedMember.Local
    // Required by EF Core
    private Preference() : this(PreferenceId.Default(), UserId.Default(), default, default, default)
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
        return new Preference(PreferenceId.New(Guid.NewGuid()), userId, GeneralPreference.CreateDefault(),
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

    public PreferenceChangeType Update(GeneralPreference generalPreference, FinancePreference financePreference,
        NotificationPreference notificationPreference)
    {
        bool isGeneralPreferencesChanged = false;
        bool isFinancePreferencesChanged = false;
        bool isNotificationPreferencesChanged = false;

        if (GeneralPreference != generalPreference)
        {
            GeneralPreference = generalPreference;
            isGeneralPreferencesChanged = true;
        }

        if (FinancePreference != financePreference)
        {
            FinancePreference = financePreference;
            isFinancePreferencesChanged = true;
        }

        if (NotificationPreference != notificationPreference)
        {
            NotificationPreference = notificationPreference;
            isNotificationPreferencesChanged = true;
        }

        PreferenceChangeType preferenceChangeType = PreferenceChangeType.Create(isGeneralPreferencesChanged,
            isFinancePreferencesChanged, isNotificationPreferencesChanged);

        return preferenceChangeType;
    }
}