namespace Expenso.TimeManagement.Core.Domain.Jobs.Model;

public sealed class JobEntryTrigger
{
    public Guid Id { get; init; }

    public string? EventType { get; init; }

    public string? EventData { get; init; }
}