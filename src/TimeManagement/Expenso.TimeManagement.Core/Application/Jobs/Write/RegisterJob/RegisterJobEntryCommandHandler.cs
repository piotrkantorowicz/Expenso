using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob.DTO.Maps;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;
using Expenso.TimeManagement.Shared.DTO.Request;
using Expenso.TimeManagement.Shared.DTO.Response;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;

internal sealed class
    RegisterJobEntryCommandHandler : ICommandHandler<RegisterJobEntryCommand, RegisterJobEntryResponse>
{
    private readonly IJobEntryRepository _jobEntryRepository;
    private readonly IJobEntryStatusRepository _jobEntryStatusRepository;
    private readonly IJobInstanceRepository _jobInstanceRepository;

    public RegisterJobEntryCommandHandler(IJobEntryRepository jobEntryRepository,
        IJobInstanceRepository jobInstanceRepository, IJobEntryStatusRepository jobEntryStatusRepository)
    {
        _jobEntryRepository =
            jobEntryRepository ?? throw new ArgumentNullException(paramName: nameof(jobEntryRepository));

        _jobEntryStatusRepository = jobEntryStatusRepository ??
                                    throw new ArgumentNullException(paramName: nameof(jobEntryStatusRepository));

        _jobInstanceRepository = jobInstanceRepository ??
                                 throw new ArgumentNullException(paramName: nameof(jobInstanceRepository));
    }

    public async Task<RegisterJobEntryResponse> HandleAsync(RegisterJobEntryCommand entryCommand,
        CancellationToken cancellationToken)
    {
        Guid jobInstanceId = JobInstance.Default.Id;

        JobInstance? jobType =
            await _jobInstanceRepository.GetAsync(id: jobInstanceId, cancellationToken: cancellationToken);

        if (jobType is null)
        {
            throw new NotFoundException(message: $"Job instance with ID {jobInstanceId} not found");
        }

        Guid jobStatusId = JobEntryStatus.Running.Id;

        JobEntryStatus? runningJobStatus =
            await _jobEntryStatusRepository.GetAsync(id: jobStatusId, cancellationToken: cancellationToken);

        if (runningJobStatus is null)
        {
            throw new NotFoundException(message: $"Job status with ID {jobStatusId} not found");
        }

        JobEntry? jobEntry = CreateJobEntry(jobEntry: entryCommand.RegisterJobEntryRequest, jobInstance: jobType,
            jobEntryStatus: runningJobStatus);

        if (jobEntry is null)
        {
            throw new NotFoundException(message: "Unable to create job entry from request");
        }

        await _jobEntryRepository.AddOrUpdateAsync(jobEntry: jobEntry, cancellationToken: cancellationToken);

        return RegisterJobEntryResponseMap.MapToJobEntry(jobEntry: jobEntry);
    }

    private static JobEntry? CreateJobEntry(RegisterJobEntryRequest? jobEntry, JobInstance? jobInstance,
        JobEntryStatus? jobEntryStatus)
    {
        if (jobEntry is null)
        {
            return null;
        }

        return new JobEntry
        {
            Id = Guid.NewGuid(),
            JobInstanceId = jobInstance?.Id ?? throw new ArgumentNullException(paramName: nameof(jobInstance)),
            CronExpression = jobEntry.Interval?.GetCronExpression(),
            RunAt = jobEntry.RunAt,
            MaxRetries = jobEntry.MaxRetries,
            JobEntryStatusId = jobEntryStatus?.Id ?? throw new ArgumentNullException(paramName: nameof(jobEntryStatus)),
            Triggers = CreateJobEntryTriggers(triggers: jobEntry.JobEntryTriggers)
        };
    }

    private static JobEntryTrigger[] CreateJobEntryTriggers(
        ICollection<RegisterJobEntryRequest_JobEntryTrigger>? triggers)
    {
        triggers ??= [];

        return triggers
            .Select(selector: x => new JobEntryTrigger
            {
                Id = Guid.NewGuid(),
                EventType = x.EventType,
                EventData = x.EventData
            })
            .ToArray();
    }
}