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
        jobEntryRepository ?? throw new ArgumentNullException(paramName: nameof(jobEntryRepository));

    private readonly IJobEntryStatusRepository _jobEntryStatusRepository = jobEntryStatusRepository ??
                                                                           throw new ArgumentNullException(
                                                                               paramName: nameof(
                                                                                   jobEntryStatusRepository));

    private readonly IJobInstanceRepository _jobInstanceRepository = jobInstanceRepository ??
                                                                     throw new ArgumentNullException(
                                                                         paramName: nameof(jobInstanceRepository));

    public async Task HandleAsync(RegisterJobCommand command, CancellationToken cancellationToken)
    {
        Guid jobInstanceId = JobInstance.Default.Id;

        JobInstance? jobType =
            await _jobInstanceRepository.GetAsync(id: jobInstanceId, cancellationToken: cancellationToken);

        if (jobType is null)
        {
            throw new NotFoundException(message: $"Job instance with id {jobInstanceId} not found");
        }

        Guid jobStatusId = JobEntryStatus.Running.Id;

        JobEntryStatus? runningJobStatus =
            await _jobEntryStatusRepository.GetAsync(id: jobStatusId, cancellationToken: cancellationToken);

        if (runningJobStatus is null)
        {
            throw new NotFoundException(message: $"Job status with id {jobStatusId} not found");
        }

        JobEntry? jobEntry =
            command.AddJobEntryRequest?.MapToJobEntry(jobInstance: jobType, jobEntryStatus: runningJobStatus);

        if (jobEntry is null)
        {
            throw new NotFoundException(message: "Unable to create job entry from request");
        }

        await _jobEntryRepository.AddOrUpdateAsync(jobEntry: jobEntry, cancellationToken: cancellationToken);
    }
}