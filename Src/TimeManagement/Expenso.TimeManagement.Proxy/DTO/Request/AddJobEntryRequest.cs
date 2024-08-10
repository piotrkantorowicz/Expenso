namespace Expenso.TimeManagement.Proxy.DTO.Request;

public sealed record AddJobEntryRequest(
    int? MaxRetries,
    ICollection<AddJobEntryRequest_JobEntryTrigger>? JobEntryTriggers,
    AddJobEntryRequest_JobEntryPeriodInterval? Interval,
    DateTimeOffset? RunAt);