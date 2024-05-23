namespace Expenso.TimeManagement.Proxy.DTO.Request;

public sealed record AddJobEntryRequest_JobEntryPeriod(
    AddJobEntryRequest_JobEntryPeriodInterval? Interval,
    int MaxRetries,
    bool Periodic);