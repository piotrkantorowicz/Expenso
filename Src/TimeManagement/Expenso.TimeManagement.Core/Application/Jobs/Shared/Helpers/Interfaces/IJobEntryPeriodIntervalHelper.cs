using Expenso.Shared.System.Types.Clock;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;

namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.Helpers.Interfaces;

internal interface IJobEntryPeriodIntervalHelper
{
    IntervalPrediction PredictInterval(JobEntryPeriod jobEntryPeriod, IClock clock);
}