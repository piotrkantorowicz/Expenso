namespace Expenso.TimeManagement.Shared.DTO.Request;

public sealed record RegisterJobEntryRequest(
    int? MaxRetries,
    ICollection<RegisterJobEntryRequest_JobEntryTrigger>? JobEntryTriggers,
    RegisterJobEntryRequest_JobEntryPeriodInterval? Interval,
    DateTimeOffset? RunAt);