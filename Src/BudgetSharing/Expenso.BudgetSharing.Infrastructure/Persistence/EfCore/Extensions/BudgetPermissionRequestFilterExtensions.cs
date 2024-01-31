using System.Linq.Expressions;

using Expenso.BudgetSharing.Application.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Extensions;

public static class BudgetPermissionRequestFilterExtensions
{
    public static Expression<Func<BudgetPermissionRequest, bool>> ToFilterExpression(
        this BudgetPermissionRequestFilter filter)
    {
        return x => filter.BudgetId != null && x.ParticipantId == filter.BudgetId && filter.ParticipantId != null &&
                    x.ParticipantId == filter.ParticipantId && filter.Status != null && x.Status == filter.Status &&
                    filter.PermissionType != null && x.PermissionType == filter.PermissionType;
    }
}