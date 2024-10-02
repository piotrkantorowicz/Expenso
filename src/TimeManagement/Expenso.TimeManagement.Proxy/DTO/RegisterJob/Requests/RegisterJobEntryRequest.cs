namespace Expenso.TimeManagement.Proxy.DTO.RegisterJob.Requests;

public sealed record RegisterJobEntryRequest(
    int? MaxRetries,
    ICollection<RegisterJobEntryRequestJobEntryTrigger>? JobEntryTriggers,
    RegisterJobEntryRequestJobEntryPeriodInterval? Interval,
    DateTimeOffset? RunAt);