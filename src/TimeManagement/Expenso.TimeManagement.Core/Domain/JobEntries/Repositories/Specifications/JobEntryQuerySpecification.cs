using System.Linq.Expressions;

using Expenso.Shared.System.Expressions.And;
using Expenso.TimeManagement.Core.Domain.JobEntries.Model;

namespace Expenso.TimeManagement.Core.Domain.JobEntries.Repositories.Specifications;

internal sealed record JobEntryQuerySpecification(
    Guid? JobEntryId = null,
    Guid? JobInstanceId = null,
    Guid[]? JobEntryStatusIds = null,
    int? MoreThanRetries = null,
    bool? IsCompleted = null,
    bool? HasRunned = null,
    bool? IsActive = null,
    bool? HasTriggers = null,
    bool? UseTracking = null,
    JobEntryIncludes? Includes = null)
{
    private readonly Guid[] _activeJobEntryStatusIds =
    [
        JobEntryStatus.Running.Id,
        JobEntryStatus.Retrying.Id
    ];

    private static readonly Dictionary<JobEntryIncludes, Expression<Func<JobEntry, object>>> JobEntryIncludesMap = new()
    {
        { JobEntryIncludes.JobEntryInstance, x => x.JobInstance! },
        { JobEntryIncludes.JobEntryStatus, x => x.JobStatus! }
    };

    public Expression<Func<JobEntry, bool>> Filter()
    {
        Expression<Func<JobEntry, bool>> predicate = p => true;

        if (JobEntryId.HasValue)
        {
            predicate = AndExpression<JobEntry>.And(leftExpression: predicate,
                rightExpression: p => p.Id == JobEntryId.Value);
        }

        if (JobInstanceId.HasValue)
        {
            predicate = AndExpression<JobEntry>.And(leftExpression: predicate,
                rightExpression: p => p.JobInstanceId == JobInstanceId.Value);
        }

        if (JobEntryStatusIds is not null && JobEntryStatusIds.Length > 0)
        {
            predicate = AndExpression<JobEntry>.And(leftExpression: predicate,
                rightExpression: p => JobEntryStatusIds.Contains(p.JobEntryStatusId));
        }

        if (MoreThanRetries.HasValue)
        {
            predicate = AndExpression<JobEntry>.And(leftExpression: predicate,
                rightExpression: p => p.CurrentRetries >= MoreThanRetries.Value);
        }

        if (IsCompleted.HasValue)
        {
            predicate = AndExpression<JobEntry>.And(leftExpression: predicate,
                rightExpression: p => p.IsCompleted == IsCompleted.Value);
        }

        if (HasRunned.HasValue)
        {
            predicate = HasRunned switch
            {
                true => AndExpression<JobEntry>.And(leftExpression: predicate, rightExpression: p => p.LastRun != null),
                false => AndExpression<JobEntry>.And(leftExpression: predicate,
                    rightExpression: p => p.LastRun == null),
                _ => predicate
            };
        }

        if (IsActive.HasValue)
        {
            predicate = IsActive switch
            {
                true => AndExpression<JobEntry>.And(leftExpression: predicate,
                    rightExpression: p =>
                        p.Triggers.Count > 0 && _activeJobEntryStatusIds.Contains(p.JobEntryStatusId)),
                false => AndExpression<JobEntry>.And(leftExpression: predicate,
                    rightExpression: p =>
                        p.Triggers.Count == 0 || !_activeJobEntryStatusIds.Contains(p.JobEntryStatusId)),
                _ => predicate
            };
        }

        if (HasTriggers.HasValue)
        {
            predicate = HasTriggers switch
            {
                true => AndExpression<JobEntry>.And(leftExpression: predicate,
                    rightExpression: p => p.Triggers.Count > 0),
                false => AndExpression<JobEntry>.And(leftExpression: predicate,
                    rightExpression: p => p.Triggers.Count == 0),
                _ => predicate
            };
        }

        return predicate;
    }

    public IEnumerable<Expression<Func<JobEntry, object>>> Include()
    {
        JobEntryIncludes includes = Includes ?? JobEntryIncludes.None;

        return JobEntryIncludesMap
            .Where(predicate: kv => includes.HasFlag(flag: kv.Key))
            .Select(selector: kv => kv.Value)
            .ToArray();
    }
}