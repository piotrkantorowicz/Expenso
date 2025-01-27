namespace Expenso.TimeManagement.Core.Domain.JobEntries.Model;

internal sealed record JobEntryStatus
{
    public static JobEntryStatus Running => new()
    {
        Id = new Guid(g: "0194a81a-48c6-7394-9b65-a820cb4ae551"),
        Name = "Running",
        Description = "The job entry is currently running"
    };

    public static JobEntryStatus Completed => new()
    {
        Id = new Guid(g: "0194a81a-48c6-71f5-8fba-bf96f4ac4c96"),
        Name = "Completed",
        Description = "The job entry has completed successfully"
    };

    public static JobEntryStatus Failed => new()
    {
        Id = new Guid(g: "0194a81a-48c6-7ac8-87b9-55ece38a30dd"),
        Name = "Failed",
        Description = "The job entry has failed"
    };

    public static JobEntryStatus Retrying => new()
    {
        Id = new Guid(g: "0194a81a-48c6-7f08-a0c6-e396f23468e4"),
        Name = "Retrying",
        Description = "The job entry is being retried"
    };

    public static JobEntryStatus Cancelled => new()
    {
        Id = new Guid(g: "0194a81a-48c6-7a92-92b9-e8455c5ed7c2"),
        Name = "Cancelled",
        Description = "The job entry has been cancelled"
    };

    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public string Description { get; init; } = null!;

    public bool IsRunning()
    {
        return Id == Running.Id;
    }

    public bool IsCompleted()
    {
        return Id == Completed.Id;
    }

    public bool IsRetrying()
    {
        return Id == Retrying.Id;
    }

    public bool IsCancelled()
    {
        return Id == Cancelled.Id;
    }

    public bool IsFailed()
    {
        return Id == Failed.Id;
    }
}