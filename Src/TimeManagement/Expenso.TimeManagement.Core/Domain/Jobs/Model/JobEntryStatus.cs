namespace Expenso.TimeManagement.Core.Domain.Jobs.Model;

internal sealed record JobEntryStatus
{
    public static JobEntryStatus Running => new()
    {
        Id = Guid.NewGuid(),
        Name = "Running",
        Description = "The job entry is currently running."
    };

    public static JobEntryStatus Completed => new()
    {
        Id = Guid.NewGuid(),
        Name = "Completed",
        Description = "The job entry has completed successfully."
    };

    public static JobEntryStatus Failed => new()
    {
        Id = Guid.NewGuid(),
        Name = "Failed",
        Description = "The job entry has failed."
    };

    public static JobEntryStatus Retrying => new()
    {
        Id = Guid.NewGuid(),
        Name = "Retrying",
        Description = "The job entry is being retried."
    };

    public static JobEntryStatus Cancelled => new()
    {
        Id = Guid.NewGuid(),
        Name = "Cancelled",
        Description = "The job entry has been cancelled."
    };

    public Guid Id { get; init; }

    public string? Name { get; init; }

    public string? Description { get; init; }
}