namespace Expenso.TimeManagement.Proxy.DTO.Request;

public sealed record AddJobEntryRequest_JobEntryPeriod(
    AddJobEntryRequest_JobEntryPeriodInterval Interval,
    DateTimeOffset RunAt);