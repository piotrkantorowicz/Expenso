using System.Linq.Expressions;

using Expenso.BudgetSharing.Application.Read.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;

using LinqKit;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Extensions;

public static class BudgetPermissionRequestFilterExtensions
{
    public static Expression<Func<BudgetPermissionRequest, bool>> ToFilterExpression(
        this BudgetPermissionRequestFilter filter)
    {
        ExpressionStarter<BudgetPermissionRequest>? predicate = PredicateBuilder.New<BudgetPermissionRequest>(true);

        if (filter.Id is not null)
        {
            predicate = predicate.And(x => x.Id == filter.Id);
        }

        if (filter.BudgetId is not null)
        {
            predicate = predicate.And(x => x.BudgetId == filter.BudgetId);
        }

        if (filter.ParticipantId is not null)
        {
            predicate = predicate.And(x => x.ParticipantId == filter.ParticipantId);
        }

        if (filter.Status != null)
        {
            predicate = predicate.And(x => x.Status == filter.Status);
        }

        if (filter.PermissionType != null)
        {
            predicate = predicate.And(x => x.PermissionType == filter.PermissionType);
        }

        return predicate;
    }
}