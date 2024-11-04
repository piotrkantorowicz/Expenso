using System.Linq.Expressions;

using Expenso.Shared.System.Expressions.And;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;

namespace Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;

internal sealed record PreferenceQuerySpecification(
    Guid? PreferenceId = null,
    Guid? UserId = null,
    bool? UseTracking = null,
    PreferenceTypes? PreferenceType = null)
{
    private static readonly Dictionary<PreferenceTypes, Expression<Func<Preference, object>>> PreferenceTypeMap = new()
    {
        { PreferenceTypes.Finance, x => x.FinancePreference! },
        { PreferenceTypes.Notification, x => x.NotificationPreference! },
        { PreferenceTypes.General, x => x.GeneralPreference! }
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
        PreferenceTypes preferenceTypes = PreferenceType ?? PreferenceTypes.None;

        return PreferenceTypeMap
            .Where(predicate: kv => preferenceTypes.HasFlag(flag: kv.Key))
            .Select(selector: kv => kv.Value)
            .ToArray();
    }
}