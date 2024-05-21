using Expenso.Shared.System.Types.Clock;
using Expenso.TimeManagement.Core.Application.Jobs.Shared.Helpers.Interfaces;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;

using NCrontab;

namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.Helpers;

internal sealed class JobEntryPeriodIntervalHelper : IJobEntryPeriodIntervalHelper
{
    public IntervalPrediction PredictInterval(JobEntryPeriod jobEntryPeriod, IClock clock)
    {
        var schedule = CrontabSchedule.Parse(jobEntryPeriod.CronExpression);
        var nextOccurrence = schedule.GetNextOccurrence(clock.UtcNow.DateTime);
        bool shouldRun = nextOccurrence == clock.UtcNow;
        
        return new IntervalPrediction(shouldRun, shouldRun ? clock.UtcNow : null);
    }
}