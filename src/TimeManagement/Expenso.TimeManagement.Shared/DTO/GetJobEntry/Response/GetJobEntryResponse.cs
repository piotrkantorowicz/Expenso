namespace Expenso.TimeManagement.Shared.DTO.GetJobEntry.Response;

public sealed record GetJobEntryResponse(
    Guid JobEntryId,
    string? CronExpression,
    int CurrentRetries,
    int MaxRetries,
    bool IsCompleted,
    DateTimeOffset? RunAt,
    DateTimeOffset? LastRun,
    GetJobEntryResponseJobInstance? JobInstance,
    GetJobEntryResponseJobEntryStatus? JobStatus,
    IEnumerable<GetJobEntryResponseJobEntryTrigger>? Triggers);