using System.Linq.Expressions;

using Expenso.Shared.System.Expressions.And;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;

namespace Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Specifications;

internal sealed record PreferenceQuerySpecification(
    Guid? PreferenceId = null,
    Guid? UserId = null,
    bool? UseTracking = null,
    PreferenceIncludes? Includes = null)
{
    private static readonly Dictionary<PreferenceIncludes, Expression<Func<Preference, object>>> PreferenceIncludesMap =
        new()
        {
            { PreferenceIncludes.Finance, x => x.FinancePreference! },
            { PreferenceIncludes.Notification, x => x.NotificationPreference! },
            { PreferenceIncludes.General, x => x.GeneralPreference! }
        };

    public Expression<Func<Preference, bool>> Filter()
    {
        Expression<Func<Preference, bool>> predicate = p => true;

        if (PreferenceId.HasValue)
        {
            predicate = AndExpression<Preference>.And(leftExpression: predicate,
                rightExpression: p => p.Id == PreferenceId.Value);
        }

        if (UserId.HasValue)
        {
            predicate = AndExpression<Preference>.And(leftExpression: predicate,
                rightExpression: p => p.UserId == UserId.Value);
        }

        return predicate;
    }

    public IEnumerable<Expression<Func<Preference, object>>> Include()
    {
        PreferenceIncludes includes = Includes ?? PreferenceIncludes.None;

        return PreferenceIncludesMap
            .Where(predicate: kv => includes.HasFlag(flag: kv.Key))
            .Select(selector: kv => kv.Value)
            .ToArray();
    }

    private bool EmptyFilter()
    {
        return (PreferenceId.HasValue || UserId.HasValue) is false;
    }
}