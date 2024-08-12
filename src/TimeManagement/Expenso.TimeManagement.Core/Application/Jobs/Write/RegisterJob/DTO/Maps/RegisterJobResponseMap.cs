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

        return new RegisterJobEntryResponse(JobEntryId: jobEntry.Id);
    }
}