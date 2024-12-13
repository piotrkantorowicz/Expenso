namespace Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Request;

public sealed record RegisterJobEntryRequest(
    int? MaxRetries,
    ICollection<RegisterJobEntryRequestJobEntryTrigger>? JobEntryTriggers,
    RegisterJobEntryRequestJobEntryPeriodInterval? Interval,
    DateTimeOffset? RunAt,
    Guid? JobEntryId = null);