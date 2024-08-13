namespace Expenso.TimeManagement.Proxy.DTO.Request;

public sealed record RegisterJobEntryRequest_JobEntryPeriodInterval(
    int? DayOfWeek = null,
    int? Month = null,
    int? DayofMonth = null,
    int? Hour = null,
    int? Minute = null,
    int? Second = null);