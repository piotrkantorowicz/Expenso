using Expenso.BudgetSharing.Application.Read.Shared.QueryStore;
using Expenso.BudgetSharing.Application.Read.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Extensions;
using Expenso.Shared.Database.EfCore.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Repositories.Read;

internal sealed class BudgetPermissionQueryStore : IBudgetPermissionQueryStore
{
    private readonly IQueryable<BudgetPermission> _budgetPermissionsQueryable;
    private readonly IBudgetSharingDbContext _budgetSharingDbContext;

    public BudgetPermissionQueryStore(IBudgetSharingDbContext budgetSharingDbContext)
    {
        ArgumentNullException.ThrowIfNull(budgetSharingDbContext);
        _budgetPermissionsQueryable = budgetSharingDbContext.BudgetPermissions.Tracking(false);
        _budgetSharingDbContext = budgetSharingDbContext;
    }

    public async Task<BudgetPermission?> SingleAsync(BudgetPermissionFilter filter,
        CancellationToken cancellationToken = default)
    {
        return await _budgetSharingDbContext
            .BudgetPermissions.Where(filter.ToFilterExpression())
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<BudgetPermission>> BrowseAsync(BudgetPermissionFilter filter,
        CancellationToken cancellationToken = default)
    {
        return await _budgetPermissionsQueryable.ToListAsync(cancellationToken);
    }
}