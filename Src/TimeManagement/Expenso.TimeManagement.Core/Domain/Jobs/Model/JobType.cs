namespace Expenso.TimeManagement.Core.Domain.Jobs.Model;

internal sealed record JobType(int Id, string? Name)
{
    // Required for EF Core
    private JobType() : this(default, default) { }
}