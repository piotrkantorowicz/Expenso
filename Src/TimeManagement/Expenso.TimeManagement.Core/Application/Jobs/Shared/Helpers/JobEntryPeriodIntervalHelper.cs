using Expenso.Shared.System.Types.Clock;
using Expenso.TimeManagement.Core.Application.Jobs.Shared.Helpers.Interfaces;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;

namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.Helpers;

internal sealed class JobEntryPeriodIntervalHelper : IJobEntryPeriodIntervalHelper
{
    public IntervalPrediction PredictInterval(JobEntryPeriod jobEntryPeriod, IClock clock)
    {
        bool shouldRun;

        switch (jobEntryPeriod.Interval)
        {
            case JobEntryPeriodInterval.Once:
                shouldRun = jobEntryPeriod.RunAt <= clock.UtcNow && jobEntryPeriod.LastRun is null;

                return new IntervalPrediction(shouldRun, shouldRun ? clock.UtcNow : null);
            case JobEntryPeriodInterval.Minutely:
                shouldRun = jobEntryPeriod.RunAt.Minute == clock.UtcNow.Minute;

                return new IntervalPrediction(shouldRun, shouldRun ? clock.UtcNow : null);
            case JobEntryPeriodInterval.Hourly:
                shouldRun = jobEntryPeriod.RunAt.Hour == clock.UtcNow.Hour;

                return new IntervalPrediction(shouldRun, shouldRun ? clock.UtcNow : null);
            case JobEntryPeriodInterval.Daily:
                shouldRun = jobEntryPeriod.RunAt.TimeOfDay <= clock.UtcNow.TimeOfDay &&
                            jobEntryPeriod.RunAt.Date.Day == clock.UtcNow.Date.Day;

                return new IntervalPrediction(shouldRun, shouldRun ? clock.UtcNow : null);
            case JobEntryPeriodInterval.Weekly:
                shouldRun = jobEntryPeriod.RunAt.TimeOfDay <= clock.UtcNow.TimeOfDay &&
                            jobEntryPeriod.RunAt.Date.DayOfWeek == clock.UtcNow.DayOfWeek;

                return new IntervalPrediction(shouldRun, shouldRun ? clock.UtcNow : null);
            case JobEntryPeriodInterval.Monthly:
                shouldRun = jobEntryPeriod.RunAt.TimeOfDay <= clock.UtcNow.TimeOfDay &&
                            jobEntryPeriod.RunAt.Day == clock.UtcNow.Day &&
                            jobEntryPeriod.RunAt.Month == clock.UtcNow.Month;

                return new IntervalPrediction(shouldRun, shouldRun ? clock.UtcNow : null);
            case JobEntryPeriodInterval.Yearly:
                shouldRun = jobEntryPeriod.RunAt.TimeOfDay <= clock.UtcNow.TimeOfDay &&
                            jobEntryPeriod.RunAt.Day == clock.UtcNow.Day &&
                            jobEntryPeriod.RunAt.Month == clock.UtcNow.Month &&
                            jobEntryPeriod.RunAt.Year == clock.UtcNow.Year;

                return new IntervalPrediction(shouldRun, shouldRun ? clock.UtcNow : null);
            default:
                throw new ArgumentOutOfRangeException(nameof(jobEntryPeriod.Interval), jobEntryPeriod.Interval,
                    $"Unknown interval: {jobEntryPeriod.Interval}");
        }
    }
}