using System.Text;

namespace Expenso.TimeManagement.Proxy.DTO.Request;

public sealed record RegisterJobEntryRequest_JobEntryPeriodInterval(
    int? DayOfWeek = null,
    int? Month = null,
    int? DayOfMonth = null,
    int? Hour = null,
    int? Minute = null,
    int? Second = null,
    bool UseSeconds = false)
{
    /*
     * 1. Second
     * 2. Minute
     * 3. Hour
     * 4. Day of month
     * 5. Month
     * 6. Day of week
     */
    public string GetCronExpression()
    {
        StringBuilder stringBuilder = new();

        if (UseSeconds)
        {
            stringBuilder.Append(value: Second?.ToString() ?? "*");
            stringBuilder.Append(value: ' ');
        }

        stringBuilder.Append(value: Minute?.ToString() ?? "*");
        stringBuilder.Append(value: ' ');
        stringBuilder.Append(value: Hour?.ToString() ?? "*");
        stringBuilder.Append(value: ' ');
        stringBuilder.Append(value: DayOfMonth?.ToString() ?? "*");
        stringBuilder.Append(value: ' ');
        stringBuilder.Append(value: Month?.ToString() ?? "*");
        stringBuilder.Append(value: ' ');
        stringBuilder.Append(value: DayOfWeek?.ToString() ?? "*");

        return stringBuilder.ToString();
    }
}