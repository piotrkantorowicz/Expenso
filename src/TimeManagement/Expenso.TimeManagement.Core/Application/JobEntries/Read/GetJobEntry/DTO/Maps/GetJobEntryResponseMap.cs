using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Shared.DTO.GetJobEntry.Response;

namespace Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntry.DTO.Maps;

internal static class GetJobEntryResponseMap
{
    public static GetJobEntryResponse MapTo(JobEntry jobEntry)
    {
        return new GetJobEntryResponse(Id: jobEntry.Id, CronExpression: jobEntry.CronExpression,
            CurrentRetries: jobEntry.CurrentRetries, MaxRetries: jobEntry.MaxRetries, IsCompleted: jobEntry.IsCompleted,
            RunAt: jobEntry.RunAt, LastRun: jobEntry.LastRun,
            JobInstance: MapJobInstance(instance: jobEntry.JobInstance),
            JobStatus: MapJobStatus(status: jobEntry.JobStatus), Triggers: MapTriggers(triggers: jobEntry.Triggers));
    }

    private static GetJobEntryResponseJobInstance? MapJobInstance(JobInstance? instance)
    {
        return instance is null
            ? null
            : new GetJobEntryResponseJobInstance(Id: instance.Id, Name: instance.Name,
                RunningDelay: instance.RunningDelay);
    }

    private static GetJobEntryResponseJobEntryStatus? MapJobStatus(JobEntryStatus? status)
    {
        return status is null
            ? null
            : new GetJobEntryResponseJobEntryStatus(Id: status.Id, Name: status.Name, Description: status.Description);
    }

    private static IEnumerable<GetJobEntryResponseJobEntryTrigger> MapTriggers(IEnumerable<JobEntryTrigger> triggers)
    {
        return triggers?.Select(selector: trigger => new GetJobEntryResponseJobEntryTrigger(
            Id: trigger.Id, EventType: trigger.EventType, EventData: trigger.EventData)) ?? [];
    }
}