namespace Expenso.TimeManagement.Core.Domain.Jobs.Model;

internal record JobEntryTrigger(string? EventType, object? EventData)
{
    // Required for EF Core
    // ReSharper disable once UnusedMember.Local
    private JobEntryTrigger() : this(default, default) { }
}