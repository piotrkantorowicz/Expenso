using System.Linq.Expressions;

using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.Shared.System.Expressions.And;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Extensions;

public static class BudgetPermissionRequestFilterExtensions
{
    public static Expression<Func<BudgetPermissionRequest, bool>> ToFilterExpression(
        this BudgetPermissionRequestFilter filter)
    {
        Expression<Func<BudgetPermissionRequest, bool>> predicate = p => true;

        if (filter.Id is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(leftExpression: predicate,
                rightExpression: x => x.Id == filter.Id);
        }

        if (filter.BudgetId is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(leftExpression: predicate,
                rightExpression: x => x.BudgetId == filter.BudgetId);
        }

        if (filter.BudgetCode is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(leftExpression: predicate,
                rightExpression: x => x.BudgetCode == filter.BudgetCode);
        }

        if (filter.ParticipantId is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(leftExpression: predicate,
                rightExpression: x => x.ParticipantId == filter.ParticipantId);
        }

        if (filter.OwnerId is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(leftExpression: predicate,
                rightExpression: x => x.OwnerId == filter.OwnerId);
        }

        if (filter.Statuses is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(leftExpression: predicate,
                rightExpression: x => filter.Statuses.Contains(x.StatusTracker.Status));
        }

        if (filter.PermissionTypes is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(leftExpression: predicate,
                rightExpression: x => filter.PermissionTypes.Contains(x.PermissionType));
        }

        return predicate;
    }
}