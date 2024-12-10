namespace Expenso.TimeManagement.Core.Application.Jobs.Read.GetJobEntries.DTO.Response;

public sealed record GetJobEntriesResponse(
    Guid Id,
    string? CronExpression,
    int CurrentRetries,
    int MaxRetries,
    bool IsCompleted,
    DateTimeOffset? RunAt,
    DateTimeOffset? LastRun,
    GetJobEntriesResponseJobInstance? JobInstance,
    GetJobEntriesResponseJobEntryStatus? JobStatus,
    IEnumerable<GetJobEntriesResponseJobEntryTrigger>? Triggers);