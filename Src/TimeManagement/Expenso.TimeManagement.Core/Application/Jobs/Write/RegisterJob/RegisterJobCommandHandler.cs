using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Const;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob.DTO.Maps;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;
using Expenso.TimeManagement.Proxy.DTO.Request;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;

internal sealed class RegisterJobCommandHandler(
    IJobEntryRepository jobEntryRepository,
    IJobTypeRepository jobTypeRepository,
    IJobStatusRepository jobStatusRepository) : ICommandHandler<RegisterJobCommand>
{
    private readonly IJobEntryRepository _jobEntryRepository =
        jobEntryRepository ?? throw new ArgumentNullException(nameof(jobEntryRepository));

    private readonly IJobStatusRepository _jobStatusRepository =
        jobStatusRepository ?? throw new ArgumentNullException(nameof(jobStatusRepository));

    private readonly IJobTypeRepository _jobTypeRepository =
        jobTypeRepository ?? throw new ArgumentNullException(nameof(jobTypeRepository));

    public async Task HandleAsync(RegisterJobCommand command, CancellationToken cancellationToken)
    {
        (_,
            (string jobTypeName, ICollection<AddJobEntryRequest_JobEntryPeriod> _,
                ICollection<AddJobEntryRequest_JobEntryTrigger> _)) = command;

        JobType? jobType = await _jobTypeRepository.GetAsync(command.AddJobEntryRequest.JobTypeName, cancellationToken);

        if (jobType is null)
        {
            throw new NotFoundException($"Job type {jobTypeName} not found.");
        }

        JobEntryStatus? jobEntryStatus =
            await _jobStatusRepository.GetAsync(JobEntryStatuses.Active, cancellationToken);

        if (jobEntryStatus is null)
        {
            throw new NotFoundException($"Job status {JobEntryStatuses.Active} not found.");
        }

        JobEntry jobEntry = command.AddJobEntryRequest.MapToJobEntry(jobType.Id, jobEntryStatus.Id);
        await _jobEntryRepository.SaveAsync(jobEntry, cancellationToken);
    }
}