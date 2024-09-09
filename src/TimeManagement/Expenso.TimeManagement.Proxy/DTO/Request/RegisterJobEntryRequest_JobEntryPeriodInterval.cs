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
        return UseSeconds
            ? $"{Second} {Minute} {Hour} {DayofMonth} {Month} {DayOfWeek}"
            : $"{Minute} {Hour} {DayofMonth} {Month} {DayOfWeek}";
    }
}