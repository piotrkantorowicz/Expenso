namespace Expenso.TimeManagement.Core.Domain.Jobs.Model;

public sealed class JobEntryPeriod
{
    public Guid Id { get; init; }
    
    public Guid JobEntryStatusId { get; init; }
    
    public string? CronExpression { get; init; }
    
    public bool Periodic { get; init; }
    public int CurrentRetries { get; set; }
    public int MaxRetries { get; init; }
    
    public JobEntryStatus? JobStatus { get; set; }
    
    public DateTimeOffset? LastRun { get; set; }
}