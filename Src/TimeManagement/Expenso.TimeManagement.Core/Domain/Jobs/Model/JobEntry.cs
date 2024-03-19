namespace Expenso.TimeManagement.Core.Domain.Jobs.Model;

internal record JobEntry(
    Guid Id,
    int JobTypeId,
    int JobStatusId,
    ICollection<JobEntryPeriod> Periods,
    ICollection<JobEntryTrigger> Triggers)
{
    // Required for EF Core
    // ReSharper disable once UnusedMember.Local
    private JobEntry() : this(default, default, default, new List<JobEntryPeriod>(), new List<JobEntryTrigger>()) { }
}