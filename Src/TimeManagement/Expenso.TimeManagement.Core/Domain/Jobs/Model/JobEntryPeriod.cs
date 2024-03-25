namespace Expenso.TimeManagement.Core.Domain.Jobs.Model;

internal sealed class JobEntryPeriod
{
    public Guid Id { get; init; }

    public JobEntryPeriodInterval Interval { get; init; }

    public DateTimeOffset RunAt { get; init; }

    public DateTimeOffset? LastRun { get; set; }

    public bool? IsCompleted { get; set; }
}