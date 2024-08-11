namespace Expenso.TimeManagement.Core.Domain.Jobs.Model;

internal sealed class JobEntry
{
    public Guid Id { get; init; }

    public Guid JobInstanceId { get; init; }

    public Guid JobEntryStatusId { get; init; }

    public string? CronExpression { get; init; }

    public DateTimeOffset? RunAt { get; init; }

    public int? CurrentRetries { get; set; }

    public int? MaxRetries { get; init; }

    public bool? IsCompleted { get; set; }

    public DateTimeOffset? LastRun { get; set; }

    public JobEntryStatus? JobStatus { get; set; }

    public JobInstance? JobInstance { get; init; }

    public ICollection<JobEntryTrigger> Triggers { get; init; } = new List<JobEntryTrigger>();
}