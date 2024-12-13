using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;
using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories.Specifications;

namespace Expenso.TimeManagement.Core.Application.JobEntries.Write.CancelJobEntry;

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
        JobEntryQuerySpecification querySpecification = new()
        {
            JobEntryId = command.Payload?.JobEntryId,
            IsActive = true,
            UseTracking = true
        };

        JobEntry? jobEntry = await _jobEntryRepository.GetJobEntryAsync(querySpecification: querySpecification,
            cancellationToken: cancellationToken);

        if (jobEntry is null)
        {
            throw new NotFoundException(resourceName: nameof(JobEntry), identifierType: IdentifierType.PrimaryId(),
                identifier: command.Payload?.JobEntryId);
        }

        Guid jobStatusId = JobEntryStatus.Cancelled.Id;

        JobEntryStatus? cancelledStatus = await _jobStatusRepository.GetAsync(id: jobStatusId, useTracking: true,
            cancellationToken: cancellationToken);

        if (cancelledStatus is null)
        {
            throw new NotFoundException(resourceName: nameof(JobEntryStatus),
                identifierType: IdentifierType.PrimaryId(), identifier: jobStatusId);
        }

        jobEntry.JobStatus = cancelledStatus;
        await _jobEntryRepository.AddOrUpdateAsync(jobEntry: jobEntry, cancellationToken: cancellationToken);
    }
}