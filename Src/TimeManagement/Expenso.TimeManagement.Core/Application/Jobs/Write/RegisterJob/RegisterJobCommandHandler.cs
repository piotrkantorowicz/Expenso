using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob.DTO.Maps;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;

internal sealed class RegisterJobCommandHandler(
    IJobEntryRepository jobEntryRepository,
    IJobInstanceRepository jobInstanceRepository,
    IJobEntryStatusRepository jobEntryStatusRepository) : ICommandHandler<RegisterJobCommand>
{
    private readonly IJobEntryRepository _jobEntryRepository =
        jobEntryRepository ?? throw new ArgumentNullException(nameof(jobEntryRepository));

    private readonly IJobEntryStatusRepository _jobEntryStatusRepository =
        jobEntryStatusRepository ?? throw new ArgumentNullException(nameof(jobEntryStatusRepository));

    private readonly IJobInstanceRepository _jobInstanceRepository =
        jobInstanceRepository ?? throw new ArgumentNullException(nameof(jobInstanceRepository));

    public async Task HandleAsync(RegisterJobCommand command, CancellationToken cancellationToken)
    {
        Guid jobInstanceId = JobInstance.Default.Id;
        JobInstance? jobType = await _jobInstanceRepository.GetAsync(jobInstanceId, cancellationToken);

        if (jobType is null)
        {
            throw new NotFoundException($"Job instance with id {jobInstanceId} not found.");
        }

        Guid jobStatusId = JobEntryStatus.Running.Id;
        JobEntryStatus? runningJobStatus = await _jobEntryStatusRepository.GetAsync(jobStatusId, cancellationToken);

        if (runningJobStatus is null)
        {
            throw new NotFoundException($"Job status with id {jobStatusId} not found.");
        }

        JobEntry? jobEntry = command.AddJobEntryRequest?.MapToJobEntry(jobType, runningJobStatus);

        if (jobEntry is null)
        {
            throw new NotFoundException("Unable to create job entry from request.");
        }

        await _jobEntryRepository.AddOrUpdateAsync(jobEntry, cancellationToken);
    }
}