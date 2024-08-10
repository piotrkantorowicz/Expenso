using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;

using Microsoft.EntityFrameworkCore;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Repositories.Write;

internal sealed class BudgetPermissionRequestRepository(IBudgetSharingDbContext budgetSharingDbContext)
    : IBudgetPermissionRequestRepository
{
    private readonly IBudgetSharingDbContext _budgetSharingDbContext = budgetSharingDbContext ??
                                                                       throw new ArgumentNullException(
                                                                           paramName: nameof(budgetSharingDbContext));

    public async Task<BudgetPermissionRequest?> GetByIdAsync(BudgetPermissionRequestId permissionId,
        CancellationToken cancellationToken)
    {
        return await _budgetSharingDbContext.BudgetPermissionRequests.SingleOrDefaultAsync(
            predicate: x => x.Id == permissionId, cancellationToken: cancellationToken);
    }

    public async Task<BudgetPermissionRequest> AddAsync(BudgetPermissionRequest permission,
        CancellationToken cancellationToken)
    {
        await _budgetSharingDbContext.BudgetPermissionRequests.AddAsync(entity: permission,
            cancellationToken: cancellationToken);

        await _budgetSharingDbContext.SaveChangesAsync(cancellationToken: cancellationToken);

        return permission;
    }

    public async Task<BudgetPermissionRequest> UpdateAsync(BudgetPermissionRequest permission,
        CancellationToken cancellationToken)
    {
        _budgetSharingDbContext.BudgetPermissionRequests.Update(entity: permission);
        await _budgetSharingDbContext.SaveChangesAsync(cancellationToken: cancellationToken);

        return permission;
    }
}