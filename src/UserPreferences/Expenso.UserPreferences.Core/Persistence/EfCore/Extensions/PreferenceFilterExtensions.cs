using System.Linq.Expressions;

using Expenso.Shared.System.Expressions.And;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;

namespace Expenso.UserPreferences.Core.Persistence.EfCore.Extensions;

internal static class PreferenceFilterExtensions
{
    public static Expression<Func<Preference, bool>> ToFilterExpression(this PreferenceFilter filter)
    {
        Expression<Func<Preference, bool>> predicate = p => true;

        if (filter.PreferenceId.HasValue)
        {
            predicate = AndExpression<Preference>.And(leftExpression: predicate,
                rightExpression: p => p.Id == filter.PreferenceId.Value);
        }

        if (filter.UserId.HasValue)
        {
            predicate = AndExpression<Preference>.And(leftExpression: predicate,
                rightExpression: p => p.UserId == filter.UserId.Value);
        }

        return predicate;
    }

    public static IEnumerable<Expression<Func<Preference, object>>> ToIncludeExpressions(this PreferenceFilter filter)
    {
        List<Expression<Func<Preference, object>>> includes = [];

        if (filter.IncludeFinancePreferences.HasValue && filter.IncludeFinancePreferences.Value)
        {
            includes.Add(item: x => x.FinancePreference!);
        }

        if (filter.IncludeNotificationPreferences.HasValue && filter.IncludeNotificationPreferences.Value)
        {
            includes.Add(item: x => x.NotificationPreference!);
        }

        if (filter.IncludeGeneralPreferences.HasValue && filter.IncludeGeneralPreferences.Value)
        {
            includes.Add(item: x => x.GeneralPreference!);
        }

        return includes.ToArray();
    }
}