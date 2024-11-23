namespace Expenso.TimeManagement.Shared.DTO.Request;

public sealed record RegisterJobEntryRequest(
    int? MaxRetries,
    ICollection<RegisterJobEntryRequestJobEntryTrigger>? JobEntryTriggers,
    RegisterJobEntryRequestJobEntryPeriodInterval? Interval,
    DateTimeOffset? RunAt);