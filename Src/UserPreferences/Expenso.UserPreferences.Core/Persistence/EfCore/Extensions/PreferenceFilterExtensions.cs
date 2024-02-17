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

        if (filter.Id.HasValue)
        {
            predicate = AndExpression<Preference>.And(predicate, p => p.Id == filter.Id.Value);
        }

        if (filter.UserId.HasValue)
        {
            predicate = AndExpression<Preference>.And(predicate, p => p.UserId == filter.UserId.Value);
        }

        return predicate;
    }

    public static IEnumerable<Expression<Func<Preference, object>>> ToIncludeExpressions(this PreferenceFilter filter)
    {
        List<Expression<Func<Preference, object>>> includes = [];

        if (filter.IncludeFinancePreferences.HasValue && filter.IncludeFinancePreferences.Value)
        {
            includes.Add(x => x.FinancePreference!);
        }

        if (filter.IncludeNotificationPreferences.HasValue && filter.IncludeNotificationPreferences.Value)
        {
            includes.Add(x => x.NotificationPreference!);
        }

        if (filter.IncludeGeneralPreferences.HasValue && filter.IncludeGeneralPreferences.Value)
        {
            includes.Add(x => x.GeneralPreference!);
        }

        return includes.ToArray();
    }
}