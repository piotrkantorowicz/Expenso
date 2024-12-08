using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Shared.DTO.GetJobEntry.Response;

namespace Expenso.TimeManagement.Core.Application.Jobs.Read.GetJobEntry.DTO.Maps;

internal static class GetJobEntryResponseMap
{
    public static GetJobEntryResponse MapTo(JobEntry jobEntry)
    {
        return new GetJobEntryResponse(JobEntryId: jobEntry.Id, CronExpression: jobEntry.CronExpression,
            CurrentRetries: jobEntry.CurrentRetries, MaxRetries: jobEntry.MaxRetries, IsCompleted: jobEntry.IsCompleted,
            RunAt: jobEntry.RunAt, LastRun: jobEntry.LastRun,
            JobInstance: new GetJobEntryResponseJobInstance(Id: jobEntry.JobInstance?.Id,
                Name: jobEntry.JobInstance?.Name, RunningDelay: jobEntry.JobInstance?.RunningDelay),
            JobStatus: new GetJobEntryResponseJobEntryStatus(Id: jobEntry.JobStatus?.Id, Name: jobEntry.JobStatus?.Name,
                Description: jobEntry.JobStatus?.Description),
            Triggers: jobEntry.Triggers.Select(selector: trigger => new GetJobEntryResponseJobEntryTrigger(
                Id: trigger.Id, EventType: trigger?.EventType, EventData: trigger?.EventData)));
    }
}