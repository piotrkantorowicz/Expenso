using System.Text;

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
                    CronExpression = ToCronExpression(x.Interval),
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
    
    private static string ToCronExpression(this AddJobEntryRequest_JobEntryPeriodInterval interval)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(interval.DayOfWeek ?? "*");
        stringBuilder.Append(' ');
        stringBuilder.Append(interval.Month ?? "*");
        stringBuilder.Append(' ');
        stringBuilder.Append(interval.DayofMonth ?? "*");
        stringBuilder.Append(' ');
        stringBuilder.Append(interval.Hour ?? "*");
        stringBuilder.Append(' ');
        stringBuilder.Append(interval.Minute ?? "*");
        stringBuilder.Append(' ');
        stringBuilder.Append(interval.Second ?? "*");
        
        return stringBuilder.ToString();
    }
}