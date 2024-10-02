namespace Expenso.TimeManagement.Proxy.DTO.RegisterJob.Responses;

public sealed record RegisterJobEntryResponse(
    Guid JobEntryId,
    Guid JobInstanceId,
    Guid JobEntryStatusId,
    string? CronExpression,
    DateTimeOffset? RunAt,
    int? CurrentRetries,
    int? MaxRetries,
    bool? IsCompleted,
    DateTimeOffset? LastRun);