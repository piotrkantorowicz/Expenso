namespace Expenso.TimeManagement.Core.Domain.Jobs.Model;

internal sealed record JobEntryPeriod(JobEntryPeriodInterval Interval, ICollection<TimeSpan> Times)
{
    // Required for EF Core
    // ReSharper disable once UnusedMember.Local
    private JobEntryPeriod() : this(default, new List<TimeSpan>()) { }
}