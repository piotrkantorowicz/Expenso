using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;
using Expenso.TimeManagement.Core.Application.JobEntries.Shared.BackgroundJobs.Events;
using Expenso.TimeManagement.Core.Application.Shared.Settings;
using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories.Specifications;
using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Request;
using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Response;

namespace Expenso.TimeManagement.Core.Application.JobEntries.Write.RegisterJobEntry;

internal sealed class
    RegisterJobEntryCommandHandler : ICommandHandler<RegisterJobEntryCommand, RegisterJobEntryResponse>
{
    private const int DefaultMaxRetries = 5;
    private readonly IEventTypeResolver _eventTypeResolver;
    private readonly IJobEntryRepository _jobEntryRepository;
    private readonly IJobEntryStatusRepository _jobEntryStatusRepository;
    private readonly IJobInstanceRepository _jobInstanceRepository;

    public RegisterJobEntryCommandHandler(IJobEntryRepository jobEntryRepository,
        IJobInstanceRepository jobInstanceRepository, IJobEntryStatusRepository jobEntryStatusRepository,
        IEventTypeResolver eventTypeResolver)
    {
        _jobEntryRepository =
            jobEntryRepository ?? throw new ArgumentNullException(paramName: nameof(jobEntryRepository));

        _jobEntryStatusRepository = jobEntryStatusRepository ??
                                    throw new ArgumentNullException(paramName: nameof(jobEntryStatusRepository));

        _eventTypeResolver = eventTypeResolver ?? throw new ArgumentNullException(paramName: nameof(eventTypeResolver));

        _jobInstanceRepository = jobInstanceRepository ??
                                 throw new ArgumentNullException(paramName: nameof(jobInstanceRepository));
    }

    public async Task<RegisterJobEntryResponse> HandleAsync(RegisterJobEntryCommand command,
        CancellationToken cancellationToken)
    {
        Guid jobInstanceId = JobInstance.Default.Id;

        JobInstance? jobType = await _jobInstanceRepository.GetAsync(id: jobInstanceId, useTracking: true,
            cancellationToken: cancellationToken);

        if (jobType is null)
        {
            throw new NotFoundException(resourceName: nameof(JobInstance), identifierType: IdentifierType.PrimaryId(),
                identifier: jobInstanceId);
        }

        Guid jobStatusId = JobEntryStatus.Running.Id;

        JobEntryStatus? runningJobStatus = await _jobEntryStatusRepository.GetAsync(id: jobStatusId, useTracking: true,
            cancellationToken: cancellationToken);

        if (runningJobStatus is null)
        {
            throw new NotFoundException(resourceName: nameof(JobEntryStatus),
                identifierType: IdentifierType.PrimaryId(), identifier: jobStatusId);
        }

        if (command.Payload?.JobEntryId is not null)
        {
            JobEntryQuerySpecification jobEntryQuerySpecification = new()
            {
                JobEntryId = command.Payload?.JobEntryId,
                UseTracking = false
            };

            JobEntry? existingJobEntry = await _jobEntryRepository.GetJobEntryAsync(
                querySpecification: jobEntryQuerySpecification, cancellationToken: cancellationToken);

            if (existingJobEntry is not null)
            {
                throw new ConflictException(
                    message: $"Job entry with id {command.Payload?.JobEntryId} already exists.");
            }
        }

        JobEntry jobEntry = CreateJobEntry(jobEntry: command.Payload, jobInstance: jobType,
            jobEntryStatus: runningJobStatus, eventTypeResolver: _eventTypeResolver);

        await _jobEntryRepository.AddOrUpdateAsync(jobEntry: jobEntry, cancellationToken: cancellationToken);

        return new RegisterJobEntryResponse(JobEntryId: jobEntry.Id);
    }

    private static JobEntry CreateJobEntry(RegisterJobEntryRequest? jobEntry, JobInstance? jobInstance,
        JobEntryStatus? jobEntryStatus, IEventTypeResolver eventTypeResolver)
    {
        return new JobEntry
        {
            Id = jobEntry?.JobEntryId ?? Guid.NewGuid(),
            JobInstanceId = jobInstance?.Id ?? throw new ArgumentNullException(paramName: nameof(jobInstance)),
            CronExpression = jobEntry?.Interval?.GetCronExpression(),
            RunAt = jobEntry?.RunAt,
            MaxRetries = jobEntry?.MaxRetries ?? DefaultMaxRetries,
            JobEntryStatusId = jobEntryStatus?.Id ?? throw new ArgumentNullException(paramName: nameof(jobEntryStatus)),
            Triggers = CreateJobEntryTriggers(triggers: jobEntry?.JobEntryTriggers,
                eventTypeResolver: eventTypeResolver)
        };
    }

    private static JobEntryTrigger[] CreateJobEntryTriggers(
        ICollection<RegisterJobEntryRequestJobEntryTrigger>? triggers, IEventTypeResolver eventTypeResolver)
    {
        triggers ??= [];

        return triggers
            .Select(selector: x =>
            {
                Type eventType = eventTypeResolver.Resolve(eventName: (AllowedEventType)x.EventType!);

                return new JobEntryTrigger
                {
                    Id = Guid.NewGuid(),
                    EventType = eventType.AssemblyQualifiedName,
                    EventData = x.EventData
                };
            })
            .ToArray();
    }
}