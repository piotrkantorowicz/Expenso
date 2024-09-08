using System.Text;

namespace Expenso.TimeManagement.Proxy.DTO.Request;

public sealed record RegisterJobEntryRequest_JobEntryPeriodInterval(
    int? DayOfWeek = null,
    int? Month = null,
    int? DayofMonth = null,
    int? Hour = null,
    int? Minute = null,
    int? Second = null,
    bool UseSeconds = false)
{
    public string GetCronExpression()
    {
        StringBuilder stringBuilder = new();
        stringBuilder.Append(value: Minute?.ToString() ?? "*");
        stringBuilder.Append(value: ' ');
        stringBuilder.Append(value: Hour?.ToString() ?? "*");
        stringBuilder.Append(value: ' ');
        stringBuilder.Append(value: DayofMonth?.ToString() ?? "*");
        stringBuilder.Append(value: ' ');
        stringBuilder.Append(value: Month?.ToString() ?? "*");
        stringBuilder.Append(value: ' ');
        stringBuilder.Append(value: DayOfWeek?.ToString() ?? "*");

        if (UseSeconds is false)
        {
            return stringBuilder.ToString();
        }

        stringBuilder.Append(value: ' ');
        stringBuilder.Append(value: Second?.ToString() ?? "*");

        return stringBuilder.ToString();
    }
}