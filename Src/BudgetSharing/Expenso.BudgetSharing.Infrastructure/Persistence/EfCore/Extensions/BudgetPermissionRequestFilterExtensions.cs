using System.Linq.Expressions;

using Expenso.BudgetSharing.Application.Read.Shared.QueryStore.Filters;
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
            predicate = AndExpression<BudgetPermissionRequest>.And(predicate, x => x.Id == filter.Id);
        }

        if (filter.BudgetId is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(predicate, x => x.BudgetId == filter.BudgetId);
        }

        if (filter.ParticipantId is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(predicate,
                x => x.ParticipantId == filter.ParticipantId);
        }

        if (filter.Status != null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(predicate, x => x.Status == filter.Status);
        }

        if (filter.PermissionType != null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(predicate,
                x => x.PermissionType == filter.PermissionType);
        }

        return predicate;
    }
}