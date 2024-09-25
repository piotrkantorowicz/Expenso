using System.Linq.Expressions;

using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.System.Expressions.And;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Extensions;

public static class BudgetPermissionFilterExtensions
{
    public static Expression<Func<BudgetPermission, bool>> ToFilterExpression(this BudgetPermissionFilter filter)
    {
        // In expressions, it is necessary to use '==' instead of 'is.' While this creates some inconsistency with the rest of the code, I don't see any viable workarounds.
        Expression<Func<BudgetPermission, bool>> predicate = p => p.Blocker == null || p.Blocker.IsBlocked == false;

        if (filter.Id is not null)
        {
            predicate = AndExpression<BudgetPermission>.And(leftExpression: predicate,
                rightExpression: x => x.Id == filter.Id);
        }

        if (filter.BudgetId is not null)
        {
            predicate = AndExpression<BudgetPermission>.And(leftExpression: predicate,
                rightExpression: x => x.BudgetId == filter.BudgetId);
        }

        if (filter.OwnerId is not null)
        {
            predicate = AndExpression<BudgetPermission>.And(leftExpression: predicate,
                rightExpression: x => x.OwnerId == filter.OwnerId);
        }

        if (filter.ParticipantId is not null)
        {
            predicate = AndExpression<BudgetPermission>.And(leftExpression: predicate,
                rightExpression: x =>
                    x.Permissions.Select(y => y.ParticipantId).Contains(PersonId.New(filter.ParticipantId.Value)));
        }

        if (filter.PermissionType is not null)
        {
            predicate = AndExpression<BudgetPermission>.And(leftExpression: predicate,
                rightExpression: x => x.Permissions.Select(y => y.PermissionType).Contains(filter.PermissionType));
        }

        return predicate;
    }

    private static bool IsBlocked(BudgetPermission budgetPermission)
    {
        return budgetPermission.Blocker?.IsBlocked is true;
    }
}