namespace Expenso.UserPreferences.Core.Domain.Preferences.Model;

internal sealed record Preference
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public GeneralPreference? GeneralPreference { get; set; }

    public FinancePreference? FinancePreference { get; set; }

    public NotificationPreference? NotificationPreference { get; set; }
}