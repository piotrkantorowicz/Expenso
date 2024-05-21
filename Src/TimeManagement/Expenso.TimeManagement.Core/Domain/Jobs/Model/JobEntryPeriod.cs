namespace Expenso.TimeManagement.Core.Domain.Jobs.Model;

internal sealed class JobEntryPeriod
{
    public Guid Id { get; init; }

    public string? CronExpression { get; init; }

    public DateTimeOffset? LastRun { get; set; }

    public bool? IsCompleted { get; set; }
}