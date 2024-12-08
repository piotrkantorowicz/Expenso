using Expenso.TimeManagement.Core.Application.Jobs.Read.GetJobEntries.DTO.Response;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;

namespace Expenso.TimeManagement.Core.Application.Jobs.Read.GetJobEntries.DTO.Maps;

internal static class GetJobEntriesResponseMap
{
    public static IReadOnlyCollection<GetJobEntriesResponse> MapTo(IReadOnlyCollection<JobEntry> jobEntries)
    {
        return jobEntries.Select(selector: MapTo).ToList();
    }

    public static GetJobEntriesResponse MapTo(JobEntry jobEntry)
    {
        return new GetJobEntriesResponse(JobEntryId: jobEntry.Id, CronExpression: jobEntry.CronExpression,
            CurrentRetries: jobEntry.CurrentRetries, MaxRetries: jobEntry.MaxRetries, IsCompleted: jobEntry.IsCompleted,
            RunAt: jobEntry.RunAt, LastRun: jobEntry.LastRun,
            JobInstance: new GetJobEntriesResponseJobInstance(Id: jobEntry.JobInstance?.Id,
                Name: jobEntry.JobInstance?.Name, RunningDelay: jobEntry.JobInstance?.RunningDelay),
            JobStatus: new GetJobEntriesResponseJobEntryStatus(Id: jobEntry.JobStatus?.Id,
                Name: jobEntry.JobStatus?.Name, Description: jobEntry.JobStatus?.Description),
            Triggers: jobEntry.Triggers.Select(selector: trigger => new GetJobEntriesResponseJobEntryTrigger(
                Id: trigger.Id, EventType: trigger?.EventType, EventData: trigger?.EventData)));
    }
}