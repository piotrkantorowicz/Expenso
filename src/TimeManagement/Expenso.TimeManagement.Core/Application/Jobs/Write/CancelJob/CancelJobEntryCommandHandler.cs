using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJob;

internal sealed class CancelJobEntryCommandHandler : ICommandHandler<CancelJobEntryCommand>
{
    private readonly IJobEntryRepository _jobEntryRepository;
    private readonly IJobEntryStatusRepository _jobStatusRepository;

    public CancelJobEntryCommandHandler(IJobEntryRepository jobEntryRepository,
        IJobEntryStatusRepository jobStatusRepository)
    {
        _jobEntryRepository =
            jobEntryRepository ?? throw new ArgumentNullException(paramName: nameof(jobEntryRepository));

        _jobStatusRepository = jobStatusRepository ??
                               throw new ArgumentNullException(paramName: nameof(jobStatusRepository));
    }

    public async Task HandleAsync(CancelJobEntryCommand command, CancellationToken cancellationToken)
    {
        Guid? jobEntryId = command.CancelJobEntryRequest?.JobEntryId;

        JobEntry? jobEntry = await _jobEntryRepository.GetJobEntry(jobEntryId: jobEntryId,
            cancellationToken: cancellationToken);

        if (jobEntry is null)
        {
            throw new NotFoundException(message: $"Job entry with id {jobEntryId} not found");
        }

        Guid jobStatusId = JobEntryStatus.Cancelled.Id;

        JobEntryStatus? cancelledStatus =
            await _jobStatusRepository.GetAsync(id: jobStatusId, cancellationToken: cancellationToken);

        if (cancelledStatus is null)
        {
            throw new NotFoundException(message: $"Job status with id {jobStatusId} not found");
        }

        jobEntry.JobStatus = cancelledStatus;
        await _jobEntryRepository.AddOrUpdateAsync(jobEntry: jobEntry, cancellationToken: cancellationToken);
    }
}