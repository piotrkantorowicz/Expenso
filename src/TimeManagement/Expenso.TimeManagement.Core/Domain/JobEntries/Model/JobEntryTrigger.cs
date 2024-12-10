namespace Expenso.TimeManagement.Core.Domain.JobEntries.Model;

internal sealed class JobEntryTrigger
{
    public Guid Id { get; init; }

    public string? EventType { get; init; }

    public string? EventData { get; init; }
}