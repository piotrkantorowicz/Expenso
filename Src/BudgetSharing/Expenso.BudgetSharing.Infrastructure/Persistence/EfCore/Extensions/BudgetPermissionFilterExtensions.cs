using System.Linq.Expressions;

using Expenso.BudgetSharing.Application.Read.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.System.Expressions.And;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Extensions;

public static class BudgetPermissionFilterExtensions
{
    public static Expression<Func<BudgetPermission, bool>> ToFilterExpression(this BudgetPermissionFilter filter)
    {
        Expression<Func<BudgetPermission, bool>> predicate = p => true;

        if (filter.Id is not null)
        {
            predicate = AndExpression<BudgetPermission>.And(predicate, x => x.Id == filter.Id);
        }

        if (filter.BudgetId is not null)
        {
            predicate = AndExpression<BudgetPermission>.And(predicate, x => x.BudgetId == filter.BudgetId);
        }

        if (filter.OwnerId is not null)
        {
            predicate = AndExpression<BudgetPermission>.And(predicate, x => x.OwnerId == filter.OwnerId);
        }

        if (filter.ParticipantId is not null)
        {
            predicate = AndExpression<BudgetPermission>.And(predicate,
                x => x.Permissions.Select(y => y.ParticipantId).Contains(PersonId.New(filter.ParticipantId.Value)));
        }

        if (filter.PermissionType != null)
        {
            predicate = AndExpression<BudgetPermission>.And(predicate,
                x => x.Permissions.Select(y => y.PermissionType).Contains(filter.PermissionType));
        }

        return predicate;
    }
}