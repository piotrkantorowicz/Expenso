using Expenso.Shared.Integration.Events;
using Expenso.Shared.Integration.MessageBroker;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.System.Types.Clock;
using Expenso.TimeManagement.Core.Application.Jobs.Shared.Helpers;
using Expenso.TimeManagement.Core.Application.Jobs.Shared.Helpers.Interfaces;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;

using Microsoft.Extensions.Logging;

namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs.JobsExecutions;

public sealed class BudgetSharingRequestsExpirationJobExecution(
    ILogger<BudgetSharingRequestsExpirationJobExecution> logger,
    IJobEntryRepository jobEntryRepository,
    IJobEntryStatusRepository jobEntryStatusRepository,
    ISerializer serializer,
    IMessageBroker messageBroker,
    IJobEntryPeriodIntervalHelper jobEntryPeriodIntervalHelper,
    IClock clock) : IBudgetSharingRequestsExpirationJobExecution
{
    private readonly ILogger<BudgetSharingRequestsExpirationJobExecution> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));
    
    private readonly IJobEntryRepository _jobEntryRepository =
        jobEntryRepository ?? throw new ArgumentNullException(nameof(jobEntryRepository));
    
    private readonly IJobEntryStatusRepository _jobEntryStatusRepository = jobEntryStatusRepository ??
                                                                           throw new ArgumentNullException(
                                                                               nameof(jobEntryStatusRepository));
    
    private readonly ISerializer _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    
    private readonly IMessageBroker _messageBroker =
        messageBroker ?? throw new ArgumentNullException(nameof(messageBroker));
    
    private readonly IJobEntryPeriodIntervalHelper _jobEntryPeriodIntervalHelper = jobEntryPeriodIntervalHelper ??
        throw new ArgumentNullException(nameof(jobEntryPeriodIntervalHelper));
    
    private readonly IClock _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    
    public async Task Execute(string jobTypeCode, CancellationToken stoppingToken)
    {
        try
        {
            IReadOnlyCollection<JobEntry> jobEntries = await _jobEntryRepository.GetActiveJobEntries(stoppingToken);
            
            if (jobEntries.Count == 0)
            {
                _logger.LogInformation("No active {JobTypeCode} job entries found", jobTypeCode);
                
                return;
            }
            
            IReadOnlyCollection<JobEntryStatus> jobStatuses = await _jobEntryStatusRepository.GetAsync(stoppingToken);
            
            if (jobStatuses.Count == 0)
            {
                _logger.LogWarning("No job entry statuses found");
                
                return;
            }
            
            foreach (JobEntry jobEntry in jobEntries)
            {
                if ((jobEntry.Periods.Any(x =>
                        x.JobStatus != JobEntryStatus.Running || x.JobStatus != JobEntryStatus.Retrying)) ||
                    (jobEntry.Triggers.Count == 0))
                {
                    _logger.LogWarning(
                        "Skipping {JobTypeName} job entry with Id {JobEntryId} because it has no periods or triggers",
                        jobTypeCode, jobEntry.Id);
                }
                
                foreach (JobEntryPeriod jobEntryPeriod in jobEntry.Periods.Where(x =>
                             x.JobStatus == JobEntryStatus.Running || x.JobStatus != JobEntryStatus.Retrying))
                {
                    try
                    {
                        IntervalPrediction intervalPrediction =
                            _jobEntryPeriodIntervalHelper.PredictInterval(jobEntryPeriod, _clock);
                        
                        if (!intervalPrediction.ShouldRun)
                        {
                            _logger.LogDebug(
                                "Skipping {JobTypeName} job entry with Id {JobEntryId} and cron {JobEntryPeriodTime} because it should not run",
                                jobTypeCode, jobEntry.Id, jobEntryPeriod.CronExpression);
                            
                            UpdateJobEntryCompletedState(jobEntry, jobEntries, jobTypeCode);
                            
                            continue;
                        }
                        
                        _logger.LogDebug(
                            "Running {JobTypeName} job entry with Id {JobEntryId} and cron {JobEntryPeriodTime}",
                            jobTypeCode, jobEntry.Id, jobEntryPeriod.CronExpression);
                        
                        List<IIntegrationEvent?>? events = jobEntry
                            .Triggers?.Select(x =>
                            {
                                if (x.EventData is null || x.EventData.Length == 0 || x.EventType is null ||
                                    Type.GetType(x.EventType) is null)
                                {
                                    _logger.LogWarning(
                                        "Skipping event trigger for {JobTypeName} job entry with Id {JobEntryId} and cron {JobEntryPeriodTime} because it is invalid",
                                        jobTypeCode, jobEntry.Id, jobEntryPeriod.CronExpression);
                                    
                                    return null;
                                }
                                
                                if (_serializer.Deserialize(x.EventData, Type.GetType(x.EventType)!) is
                                    IIntegrationEvent
                                    @event)
                                {
                                    return @event;
                                }
                                
                                _logger.LogWarning(
                                    "Failed to deserialize event trigger for {JobTypeName} job entry with Id {JobEntryId} and cron {JobEntryPeriodTime}",
                                    jobTypeCode, jobEntry.Id, jobEntryPeriod.CronExpression);
                                
                                return null;
                            })
                            .Where(x => x is not null)
                            .ToList();
                        
                        if (events?.Count > 0)
                        {
                            foreach (IIntegrationEvent? integrationEvent in events)
                            {
                                await _messageBroker.PublishAsync(integrationEvent!, stoppingToken);
                            }
                            
                            _logger.LogDebug(
                                "Published {EventsCount} events for {JobTypeName} job entry with Id {JobEntryId} and cron {JobEntryPeriodTime}",
                                events.Count, jobTypeCode, jobEntry.Id, jobEntryPeriod.CronExpression);
                        }
                        
                        jobEntryPeriod.LastRun = intervalPrediction.LastRun;
                        
                        if (!jobEntryPeriod.Periodic)
                        {
                            jobEntryPeriod.JobStatus = jobStatuses.First(x => x.Id == JobEntryStatus.Completed.Id);
                        }
                        
                        UpdateJobEntryCompletedState(jobEntry, jobEntries, jobTypeCode);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex,
                            "An error occurred while processing {JobTypeName} and job entry with Id {JobEntryId} and job entry period with Id {JobEntryPeriodId} and cron {JobEntryPeriodTime}",
                            jobTypeCode, jobEntry.Id, jobEntryPeriod.Id, jobEntryPeriod.CronExpression);
                        
                        jobEntryPeriod.CurrentRetries++;
                        
                        jobEntryPeriod.JobStatus = jobEntryPeriod.MaxRetries >= jobEntryPeriod.CurrentRetries
                            ? jobStatuses.First(x => x.Id == JobEntryStatus.Failed.Id)
                            : jobStatuses.First(x => x.Id == JobEntryStatus.Retrying.Id);
                    }
                    finally
                    {
                        await _jobEntryRepository.SaveAsync(jobEntry, stoppingToken);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing {JobTypeName}", jobTypeCode);
        }
    }
    
    private void UpdateJobEntryCompletedState(JobEntry currentJobEntry, IEnumerable<JobEntry> jobEntries,
        string jobTypeCode)
    {
        if (!jobEntries.Any(x => x.Periods.All(y =>
                y.JobStatus == JobEntryStatus.Completed || y.JobStatus == JobEntryStatus.Cancelled ||
                y.JobStatus == JobEntryStatus.Failed)))
        {
            return;
        }
        
        _logger.LogDebug("All periods for {JobTypeName} job entry with Id {JobEntryId} have been completed",
            jobTypeCode, currentJobEntry.Id);
        
        currentJobEntry.IsCompleted = true;
    }
}