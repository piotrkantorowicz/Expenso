using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Proxy.DTO.Response;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob.DTO.Maps;

internal static class RegisterJobResponseMap
{
    public static RegisterJobEntryResponse? MapToJobEntry(JobEntry? jobEntry)
    {
        if (jobEntry is null)
        {
            return null;
        }

        return new RegisterJobEntryResponse(JobEntryId: jobEntry.Id, JobInstanceId: jobEntry.JobInstanceId,
            JobEntryStatusId: jobEntry.JobEntryStatusId, CronExpression: jobEntry.CronExpression, RunAt: jobEntry.RunAt,
            CurrentRetries: jobEntry.CurrentRetries, MaxRetries: jobEntry.MaxRetries, IsCompleted: jobEntry.IsCompleted,
            LastRun: jobEntry.LastRun);
    }
}