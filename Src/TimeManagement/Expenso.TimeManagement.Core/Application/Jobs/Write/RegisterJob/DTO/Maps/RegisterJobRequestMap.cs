using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Proxy.DTO.Request;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob.DTO.Maps;

internal static class RegisterJobRequestMap
{
    public static JobEntry MapToJobEntry(this AddJobEntryRequest request, int jobTypeId, int jobStatusId)
    {
        return new JobEntry(Guid.NewGuid(), jobTypeId, jobStatusId,
            request
                .JobEntryPeriods.Select(x => new JobEntryPeriod((JobEntryPeriodInterval)x.Interval, x.Times))
                .ToList(),
            request.JobEntryTriggers.Select(x => new JobEntryTrigger(x.EventType, x.EventData)).ToList());
    }
}