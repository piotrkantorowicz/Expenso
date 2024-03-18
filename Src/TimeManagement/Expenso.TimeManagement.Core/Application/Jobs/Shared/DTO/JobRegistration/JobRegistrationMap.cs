using Expenso.TimeManagement.Core.Domain.Jobs.Model;

namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.DTO.JobRegistration;

internal static class JobRegistrationMap
{
    public static JobRegistration ToJobRegistration(this JobEntry jobEntry)
    {
        return new JobRegistration();
    }
}