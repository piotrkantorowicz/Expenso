namespace Expenso.TimeManagement.Core.Domain.Jobs.Model;

internal record JobEntryTrigger(string? EventType, object? EventData)
{
    // Required for EF Core
    private JobEntryTrigger() : this(default, default) { }
}