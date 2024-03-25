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
        (_,
            (string jobTypeName, ICollection<AddJobEntryRequest_JobEntryPeriod> _,
                ICollection<AddJobEntryRequest_JobEntryTrigger> _)) = command;

        JobEntryType? jobType =
            await _jobEntryTypeRepository.GetAsync(command.AddJobEntryRequest.JobTypeNameCode, cancellationToken);

        if (jobType is null)
        {
            throw new NotFoundException($"Job type {jobTypeName} not found.");
        }

        JobEntryStatus? jobEntryStatus =
            await _jobEntryStatusRepository.GetAsync(JobEntryStatuses.Active, cancellationToken);

        if (jobEntryStatus is null)
        {
            throw new NotFoundException($"Job status {JobEntryStatuses.Active} not found.");
        }

        JobEntry jobEntry = command.AddJobEntryRequest.MapToJobEntry(jobType, jobEntryStatus);
        await _jobEntryRepository.SaveAsync(jobEntry, cancellationToken);
    }
}