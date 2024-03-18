namespace Expenso.TimeManagement.Core.Domain.Jobs.Model;

internal sealed record JobEntryPeriod(JobPeriodInterval Interval, ICollection<TimeSpan> Times)
{
    // Required for EF Core
    private JobEntryPeriod() : this(default, new List<TimeSpan>()) { }
}

public enum JobPeriodInterval
{
    Once,
    Daily,
    Weekly,
    Monthly,
    Yearly
}