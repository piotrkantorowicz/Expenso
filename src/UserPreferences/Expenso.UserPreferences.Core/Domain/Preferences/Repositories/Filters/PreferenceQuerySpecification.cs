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
        List<Expression<Func<Preference, object>>> includes = [];
        PreferenceTypes preferenceTypes = PreferenceType ?? PreferenceTypes.None;

        if (preferenceTypes.HasFlag(flag: PreferenceTypes.Finance))
        {
            includes.Add(item: x => x.FinancePreference!);
        }

        if (preferenceTypes.HasFlag(flag: PreferenceTypes.Notification))
        {
            includes.Add(item: x => x.NotificationPreference!);
        }

        if (preferenceTypes.HasFlag(flag: PreferenceTypes.General))
        {
            includes.Add(item: x => x.GeneralPreference!);
        }

        return includes.ToArray();
    }
}