using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob.DTO.Maps;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;

internal sealed class RegisterJobCommandHandler(
    IJobEntryRepository jobEntryRepository,
    IJobEntryTypeRepository jobEntryTypeRepository,
    IJobEntryStatusRepository jobEntryStatusRepository) : ICommandHandler<RegisterJobCommand>
{
    private readonly IJobEntryRepository _jobEntryRepository =
        jobEntryRepository ?? throw new ArgumentNullException(nameof(jobEntryRepository));
    
    private readonly IJobEntryStatusRepository _jobEntryStatusRepository =
        jobEntryStatusRepository ?? throw new ArgumentNullException(nameof(jobEntryStatusRepository));
    
    private readonly IJobEntryTypeRepository _jobEntryTypeRepository =
        jobEntryTypeRepository ?? throw new ArgumentNullException(nameof(jobEntryTypeRepository));
    
    public async Task HandleAsync(RegisterJobCommand command, CancellationToken cancellationToken)
    {
        JobEntryType? jobType =
            await _jobEntryTypeRepository.GetAsync(command.AddJobEntryRequest?.JobTypeNameCode, cancellationToken);
        
        if (jobType is null)
        {
            throw new NotFoundException($"Job type {command.AddJobEntryRequest?.JobTypeNameCode} not found.");
        }
        
        JobEntryStatus? runningJobStatus =
            await _jobEntryStatusRepository.GetAsync(JobEntryStatus.Running.Name, cancellationToken);
        
        if (runningJobStatus is null)
        {
            throw new NotFoundException($"Job status {JobEntryStatus.Running.Name} not found.");
        }
        
        JobEntry? jobEntry = command.AddJobEntryRequest?.MapToJobEntry(jobType, runningJobStatus);
        
        if (jobEntry is null)
        {
            throw new NotFoundException("Unable to create job entry from request.");
        }
        
        await _jobEntryRepository.AddOrUpdateAsync(jobEntry, cancellationToken);
    }
}