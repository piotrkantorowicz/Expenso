using Expenso.TimeManagement.Core.Application.Jobs.Shared.DTO.JobRegistration;

namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.Registry;

internal interface IJobRegistry
{
    public void RegisterJob(JobRegistration jobRegistration);
}