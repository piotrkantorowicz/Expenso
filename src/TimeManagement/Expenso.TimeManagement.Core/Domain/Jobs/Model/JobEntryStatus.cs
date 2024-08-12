namespace Expenso.TimeManagement.Core.Domain.Jobs.Model;

internal sealed record JobEntryStatus
{
    public static JobEntryStatus Running => new()
    {
        Id = new Guid(g: "53b12b3e-1db8-4792-9e24-a4da5f3e5ba3"),
        Name = "Running",
        Description = "The job entry is currently running"
    };

    public static JobEntryStatus Completed => new()
    {
        Id = new Guid(g: "6d2e7d7e-7e65-483c-93ad-72f83cc22cb1"),
        Name = "Completed",
        Description = "The job entry has completed successfully"
    };

    public static JobEntryStatus Failed => new()
    {
        Id = new Guid(g: "2f3a0e2f-8531-493a-9086-3e8faf01df95"),
        Name = "Failed",
        Description = "The job entry has failed"
    };

    public static JobEntryStatus Retrying => new()
    {
        Id = new Guid(g: "cad1ccaf-a790-4e45-a8fd-5135ddf5b028"),
        Name = "Retrying",
        Description = "The job entry is being retried"
    };

    public static JobEntryStatus Cancelled => new()
    {
        Id = new Guid(g: "ac5b3c2d-8bdf-4629-ad8e-dbd2106949e3"),
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