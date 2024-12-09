using Expenso.TimeManagement.Core.Application.Jobs.Read.GetJobEntries.DTO.Response;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;

namespace Expenso.TimeManagement.Core.Application.Jobs.Read.GetJobEntries.DTO.Maps;

internal static class GetJobEntriesResponseMap
{
    public static IReadOnlyCollection<GetJobEntriesResponse> MapTo(IReadOnlyCollection<JobEntry> jobEntries)
    {
        return jobEntries.Select(selector: MapTo).ToList();
    }

    private static GetJobEntriesResponse MapTo(JobEntry jobEntry)
    {
        return new GetJobEntriesResponse(JobEntryId: jobEntry.Id, CronExpression: jobEntry.CronExpression,
            CurrentRetries: jobEntry.CurrentRetries, MaxRetries: jobEntry.MaxRetries, IsCompleted: jobEntry.IsCompleted,
            RunAt: jobEntry.RunAt, LastRun: jobEntry.LastRun,
            JobInstance: MapJobInstance(instance: jobEntry.JobInstance),
            JobStatus: MapJobStatus(status: jobEntry.JobStatus), Triggers: MapTriggers(triggers: jobEntry.Triggers));
    }

    private static GetJobEntriesResponseJobInstance? MapJobInstance(JobInstance? instance)
    {
        return instance is null
            ? null
            : new GetJobEntriesResponseJobInstance(Id: instance.Id, Name: instance.Name,
                RunningDelay: instance.RunningDelay);
    }

    private static GetJobEntriesResponseJobEntryStatus? MapJobStatus(JobEntryStatus? status)
    {
        return status is null
            ? null
            : new GetJobEntriesResponseJobEntryStatus(Id: status.Id, Name: status.Name,
                Description: status.Description);
    }

    private static IEnumerable<GetJobEntriesResponseJobEntryTrigger> MapTriggers(IEnumerable<JobEntryTrigger> triggers)
    {
        return triggers?.Select(selector: trigger => new GetJobEntriesResponseJobEntryTrigger(
            Id: trigger.Id, EventType: trigger.EventType, EventData: trigger.EventData)) ?? [];
    }
}