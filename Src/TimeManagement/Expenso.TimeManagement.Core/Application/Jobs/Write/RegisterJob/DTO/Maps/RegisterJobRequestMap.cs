using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Proxy.DTO.Request;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob.DTO.Maps;

internal static class RegisterJobRequestMap
{
    public static JobEntry MapToJobEntry(this AddJobEntryRequest request, JobEntryType jobEntryType,
        JobEntryStatus jobEntryStatus)
    {
        return new JobEntry
        {
            Id = Guid.NewGuid(),
            JobEntryTypeId = jobEntryType.Id,
            JobEntryStatusId = jobEntryStatus.Id,
            JobEntryType = jobEntryType,
            JobStatus = jobEntryStatus,
            Periods = request
                .JobEntryPeriods.Select(x => new JobEntryPeriod
                {
                    Id = Guid.NewGuid(),
                    Interval = (JobEntryPeriodInterval)x.Interval,
                    RunAt = x.RunAt
                })
                .ToList(),
            Triggers = request
                .JobEntryTriggers.Select(x => new JobEntryTrigger
                {
                    Id = Guid.NewGuid(),
                    EventType = x.EventType,
                    EventData = x.EventData
                })
                .ToList()
        };
    }
}