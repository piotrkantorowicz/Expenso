namespace Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

internal sealed record NotificationPreference
{
    private NotificationPreference(bool sendFinanceReportEnabled, int sendFinanceReportInterval)
    {
        SendFinanceReportEnabled = sendFinanceReportEnabled;
        SendFinanceReportInterval = sendFinanceReportInterval;
    }

    public int SendFinanceReportInterval { get; }

    public bool SendFinanceReportEnabled { get; }

    public static NotificationPreference CreateDefault()
    {
        return new NotificationPreference(true, 7);
    }

    public static NotificationPreference Create(bool sendFinanceReportEnabled, int sendFinanceReportInterval)
    {
        return new NotificationPreference(sendFinanceReportEnabled, sendFinanceReportInterval);
    }
}