namespace Expenso.TimeManagement.Core.Domain.Jobs.Model;

internal sealed record JobEntryStatus(int Id, string? Name, string? Reason)
{
    // Required for EF Core
    private JobEntryStatus() : this(default, default, default) { }
}