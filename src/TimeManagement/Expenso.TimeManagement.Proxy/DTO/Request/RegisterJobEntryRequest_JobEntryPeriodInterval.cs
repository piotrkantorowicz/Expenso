namespace Expenso.TimeManagement.Proxy.DTO.Request;

public sealed record RegisterJobEntryRequest_JobEntryPeriodInterval(
    string? DayOfWeek = null,
    string? Month = null,
    string? DayofMonth = null,
    string? Hour = null,
    string? Minute = null,
    string? Second = null);