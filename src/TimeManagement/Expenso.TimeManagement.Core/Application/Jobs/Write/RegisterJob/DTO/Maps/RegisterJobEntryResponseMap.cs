using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Shared.DTO.Response;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob.DTO.Maps;

internal static class RegisterJobEntryResponseMap
{
    public static RegisterJobEntryResponse MapToJobEntry(JobEntry jobEntry)
    {
        return new RegisterJobEntryResponse(JobEntryId: jobEntry.Id, JobInstanceId: jobEntry.JobInstanceId,
            JobEntryStatusId: jobEntry.JobEntryStatusId, CronExpression: jobEntry.CronExpression, RunAt: jobEntry.RunAt,
            CurrentRetries: jobEntry.CurrentRetries, MaxRetries: jobEntry.MaxRetries, IsCompleted: jobEntry.IsCompleted,
            LastRun: jobEntry.LastRun);
    }
}