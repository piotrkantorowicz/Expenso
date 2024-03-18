namespace Expenso.TimeManagement.Core.Domain.Jobs.Model;

internal record JobEntry(
    Guid Id,
    int JobTypeId,
    JobEntryStatus? Status,
    ICollection<JobEntryPeriod> Periods,
    ICollection<JobEntryTrigger> Triggers)
{
    // Required for EF Core
    private JobEntry() : this(default, default, default, new List<JobEntryPeriod>(), new List<JobEntryTrigger>()) { }
}