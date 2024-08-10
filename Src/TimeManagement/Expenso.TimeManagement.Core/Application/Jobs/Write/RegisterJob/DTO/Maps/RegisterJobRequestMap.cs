using System.Text;

using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Proxy.DTO.Request;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob.DTO.Maps;

internal static class RegisterJobRequestMap
{
    private const string DefaultCronExpression = "* * * * * *";

    public static JobEntry? MapToJobEntry(this AddJobEntryRequest? jobEntry, JobInstance? jobInstance,
        JobEntryStatus? jobEntryStatus)
    {
        if (jobEntry is null)
        {
            return null;
        }

        return new JobEntry
        {
            Id = Guid.NewGuid(),
            JobInstanceId = jobInstance?.Id ?? throw new ArgumentNullException(nameof(jobInstance)),
            CronExpression = ToCronExpression(jobEntry.Interval),
            RunAt = jobEntry.RunAt,
            MaxRetries = jobEntry.MaxRetries,
            JobEntryStatusId = jobEntryStatus?.Id ?? throw new ArgumentNullException(nameof(jobEntryStatus)),
            Triggers = jobEntry.JobEntryTriggers?.MapToJobEntryTriggers() ?? new List<JobEntryTrigger>()
        };
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