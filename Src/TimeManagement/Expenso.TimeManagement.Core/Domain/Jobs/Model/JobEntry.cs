namespace Expenso.TimeManagement.Core.Domain.Jobs.Model;

internal sealed class JobEntry
{
    public Guid Id { get; init; }

    public Guid JobEntryTypeId { get; init; }

    public Guid JobEntryStatusId { get; init; }

    public JobEntryType? JobEntryType { get; init; }

    public JobEntryStatus? JobStatus { get; set; }

    public ICollection<JobEntryPeriod> Periods { get; init; } = new List<JobEntryPeriod>();

    public ICollection<JobEntryTrigger> Triggers { get; init; } = new List<JobEntryTrigger>();
}