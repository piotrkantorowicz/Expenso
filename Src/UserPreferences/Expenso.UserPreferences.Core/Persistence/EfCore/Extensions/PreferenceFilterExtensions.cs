using System.Linq.Expressions;

using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;

using LinqKit;

namespace Expenso.UserPreferences.Core.Persistence.EfCore.Extensions;

internal static class PreferenceFilterExtensions
{
    public static Expression<Func<Preference, bool>> ToFilterExpression(this PreferenceFilter filter)
    {
        ExpressionStarter<Preference>? predicate = PredicateBuilder.New<Preference>(true);

        if (filter.Id.HasValue)
        {
            predicate = predicate.And(x => x.Id == filter.Id);
        }

        if (filter.UserId.HasValue)
        {
            predicate = predicate.And(x => x.UserId == filter.UserId);
        }

        return predicate;
    }

    public static Expression<Func<Preference, object>>[] ToIncludeExpressions(this PreferenceFilter filter)
    {
        List<Expression<Func<Preference, object>>> includes = new();

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