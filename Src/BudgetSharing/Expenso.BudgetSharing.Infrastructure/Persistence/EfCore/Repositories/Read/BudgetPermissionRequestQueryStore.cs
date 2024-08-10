using Expenso.BudgetSharing.Application.Shared.QueryStore;
using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Extensions;
using Expenso.Shared.Database.EfCore.Queryable;

using Microsoft.EntityFrameworkCore;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Repositories.Read;

internal sealed class BudgetPermissionRequestQueryStore : IBudgetPermissionRequestQueryStore
{
    private readonly IQueryable<BudgetPermissionRequest> _budgetPermissionRequestsQueryable;

    public BudgetPermissionRequestQueryStore(IBudgetSharingDbContext budgetSharingDbContext)
    {
        ArgumentNullException.ThrowIfNull(budgetSharingDbContext);
        _budgetPermissionRequestsQueryable = budgetSharingDbContext.BudgetPermissionRequests.Tracking(false);
    }

    public async Task<BudgetPermissionRequest?> SingleAsync(BudgetPermissionRequestFilter filter,
        CancellationToken cancellationToken)
    {
        return await _budgetPermissionRequestsQueryable
            .Where(filter.ToFilterExpression())
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<BudgetPermissionRequest>> Browse(BudgetPermissionRequestFilter filter,
        CancellationToken cancellationToken)
    {
        return await _budgetPermissionRequestsQueryable
            .Where(filter.ToFilterExpression())
            .ToListAsync(cancellationToken);
    }
}