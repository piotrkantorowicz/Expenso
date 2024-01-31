using System.Linq.Expressions;

using Expenso.BudgetSharing.Application.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Model;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Extensions;

public static class BudgetPermissionFilterExtensions
{
    public static Expression<Func<BudgetPermission, bool>> ToFilterExpression(this BudgetPermissionFilter filter)
    {
        return x => filter.Id != null && x.BudgetId == filter.Id && filter.BudgetId != null &&
                    x.BudgetId == filter.BudgetId && filter.ParticipantId != null &&
                    x.Permissions.Select(y => y.ParticipantId).Contains(PersonId.Create(filter.ParticipantId.Value)) &&
                    filter.PermissionType != null &&
                    x.Permissions.Select(y => y.PermissionType).Contains(filter.PermissionType);
    }

    private static Expression<Func<BudgetPermission, object>>? ToIncludeExpression(this BudgetPermissionFilter filter)
    {
        if (filter.IncludePermissions == true)
        {
            return x => x.Permissions;
        }

        return null;
    }
}