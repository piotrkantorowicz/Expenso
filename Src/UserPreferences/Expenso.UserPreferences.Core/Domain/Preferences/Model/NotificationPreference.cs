namespace Expenso.UserPreferences.Core.Domain.Preferences.Model;

internal sealed record NotificationPreference
{
    public Guid Id { get; init; }

    public Guid PreferenceId { get; init; }

    public int SendFinanceReportInterval { get; init; }

    public bool SendFinanceReportEnabled { get; init; }
}