using Expenso.TimeManagement.Core.Domain.Jobs.Model;

namespace Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

internal interface IJobEntryTypeRepository
{
    Task<JobEntryType?> GetAsync(string code, CancellationToken cancellationToken);
}