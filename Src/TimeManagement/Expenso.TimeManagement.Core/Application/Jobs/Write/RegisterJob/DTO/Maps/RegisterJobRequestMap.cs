using System.Text;

using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Proxy.DTO.Request;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob.DTO.Maps;

internal static class RegisterJobRequestMap
{
    private const string DefaultCronExpression = "* * * * * *";
    
    public static JobEntry MapToJobEntry(this AddJobEntryRequest? request, JobEntryType? jobEntryType,
        JobEntryStatus? jobEntryStatus)
    {
        return new JobEntry
        {
            Id = Guid.NewGuid(),
            JobEntryTypeId = jobEntryType?.Id ?? throw new ArgumentNullException(nameof(jobEntryType)),
            JobEntryType = jobEntryType,
            Periods = request?.JobEntryPeriods?.MapToJobEntryPeriods(jobEntryStatus) ?? new List<JobEntryPeriod>(),
            Triggers = request?.JobEntryTriggers?.MapToJobEntryTriggers() ?? new List<JobEntryTrigger>()
        };
    }
    
    private static ICollection<JobEntryPeriod> MapToJobEntryPeriods(
        this ICollection<AddJobEntryRequest_JobEntryPeriod>? periods, JobEntryStatus? jobEntryStatus)
    {
        if (periods is null)
        {
            return new List<JobEntryPeriod>();
        }
        
        return periods
            .Select(x => new JobEntryPeriod
            {
                Id = Guid.NewGuid(),
                CronExpression = ToCronExpression(x.Interval),
                MaxRetries = x.MaxRetries,
                JobEntryStatusId = jobEntryStatus?.Id ?? throw new ArgumentNullException(nameof(jobEntryStatus)),
                JobStatus = jobEntryStatus,
                Periodic = x.Periodic
            })
            .ToArray();
    }
    
    private static ICollection<JobEntryTrigger> MapToJobEntryTriggers(
        this ICollection<AddJobEntryRequest_JobEntryTrigger>? triggers)
    {
        if (triggers is null)
        {
            return new List<JobEntryTrigger>();
        }
        
        return triggers
            .Select(x => new JobEntryTrigger
            {
                Id = Guid.NewGuid(),
                EventType = x.EventType,
                EventData = x.EventData
            })
            .ToArray();
    }
    
    private static string ToCronExpression(this AddJobEntryRequest_JobEntryPeriodInterval? interval)
    {
        if (interval is null)
        {
            return DefaultCronExpression;
        }
        
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