using System.Linq.Expressions;

using Expenso.BudgetSharing.Application.Read.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

using LinqKit;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Extensions;

public static class BudgetPermissionFilterExtensions
{
    public static Expression<Func<BudgetPermission, bool>> ToFilterExpression(this BudgetPermissionFilter filter)
    {
        ExpressionStarter<BudgetPermission>? predicate = PredicateBuilder.New<BudgetPermission>(true);

        if (filter.Id.HasValue)
        {
            predicate = predicate.And(x => x.Id == filter.Id);
        }

        if (filter.BudgetId.HasValue)
        {
            predicate = predicate.And(x => x.BudgetId == filter.BudgetId);
        }

        if (filter.OwnerId.HasValue)
        {
            predicate = predicate.And(x => x.OwnerId == filter.OwnerId);
        }

        if (filter.ParticipantId.HasValue)
        {
            predicate = predicate.And(x =>
                x.Permissions.Select(y => y.ParticipantId).Contains(PersonId.Create(filter.ParticipantId.Value)));
        }

        if (filter.PermissionType != null)
        {
            predicate = predicate.And(x => x.Permissions.Select(y => y.PermissionType).Contains(filter.PermissionType));
        }

        return predicate;
    }
}