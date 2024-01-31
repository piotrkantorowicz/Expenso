using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model.ValueObjects;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Repositories;

internal sealed class BudgetPermissionRequestRepository(IBudgetSharingDbContext budgetSharingDbContext)
    : IBudgetPermissionRequestRepository
{
    private readonly IBudgetSharingDbContext _budgetSharingDbContext =
        budgetSharingDbContext ?? throw new ArgumentNullException(nameof(budgetSharingDbContext));

    public async Task<BudgetPermissionRequest?> GetByIdAsync(BudgetPermissionRequestId permissionId,
        CancellationToken cancellationToken = default)
    {
        return await _budgetSharingDbContext.BudgetPermissionRequests.SingleOrDefaultAsync(x => x.Id == permissionId,
            cancellationToken);
    }

    public async Task<BudgetPermissionRequest> AddAsync(BudgetPermissionRequest permission,
        CancellationToken cancellationToken)
    {
        await _budgetSharingDbContext.BudgetPermissionRequests.AddAsync(permission, cancellationToken);
        await _budgetSharingDbContext.SaveChangesAsync(cancellationToken);

        return permission;
    }

    public async Task<BudgetPermissionRequest> UpdateAsync(BudgetPermissionRequest permission,
        CancellationToken cancellationToken)
    {
        _budgetSharingDbContext.BudgetPermissionRequests.Update(permission);
        await _budgetSharingDbContext.SaveChangesAsync(cancellationToken);

        return permission;
    }
}