using System.Linq.Expressions;

using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.Shared.System.Expressions.And;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Extensions;

public static class BudgetPermissionRequestFilterExtensions
{
    public static Expression<Func<BudgetPermissionRequest, bool>> ToFilterExpression(
        this BudgetPermissionRequestQuerySpecification querySpecification)
    {
        Expression<Func<BudgetPermissionRequest, bool>> predicate = p => true;

        if (querySpecification.Id is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(leftExpression: predicate,
                rightExpression: x => x.Id == querySpecification.Id);
        }

        if (querySpecification.BudgetId is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(leftExpression: predicate,
                rightExpression: x => x.BudgetId == querySpecification.BudgetId);
        }

        if (querySpecification.BudgetCode is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(leftExpression: predicate,
                rightExpression: x => x.BudgetCode == querySpecification.BudgetCode);
        }

        if (querySpecification.ParticipantId is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(leftExpression: predicate,
                rightExpression: x => x.ParticipantId == querySpecification.ParticipantId);
        }

        if (querySpecification.OwnerId is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(leftExpression: predicate,
                rightExpression: x => x.OwnerId == querySpecification.OwnerId);
        }

        if (querySpecification.Statuses is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(leftExpression: predicate,
                rightExpression: x => querySpecification.Statuses.Contains(x.StatusTracker.Status));
        }

        if (querySpecification.PermissionTypes is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(leftExpression: predicate,
                rightExpression: x => querySpecification.PermissionTypes.Contains(x.PermissionType));
        }

        return predicate;
    }
}