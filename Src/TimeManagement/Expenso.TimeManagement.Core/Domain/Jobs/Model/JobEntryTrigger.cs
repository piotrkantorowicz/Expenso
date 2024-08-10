namespace Expenso.TimeManagement.Core.Domain.Jobs.Model;

internal sealed class JobEntryTrigger
{
    public Guid Id { get; init; }

    public string? EventType { get; init; }

    public string? EventData { get; init; }
}