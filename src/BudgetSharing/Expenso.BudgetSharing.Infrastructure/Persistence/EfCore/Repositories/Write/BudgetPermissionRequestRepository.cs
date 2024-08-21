using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

using Microsoft.EntityFrameworkCore;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Repositories.Write;

internal sealed class BudgetPermissionRequestRepository : IBudgetPermissionRequestRepository
{
    private readonly IBudgetSharingDbContext _budgetSharingDbContext;

    public BudgetPermissionRequestRepository(IBudgetSharingDbContext budgetSharingDbContext)
    {
        _budgetSharingDbContext = budgetSharingDbContext ??
                                  throw new ArgumentNullException(paramName: nameof(budgetSharingDbContext));
    }

    public async Task<BudgetPermissionRequest?> GetByIdAsync(BudgetPermissionRequestId permissionId,
        CancellationToken cancellationToken)
    {
        return await _budgetSharingDbContext.BudgetPermissionRequests.SingleOrDefaultAsync(
            predicate: x => x.Id == permissionId, cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyCollection<BudgetPermissionRequest>> GetUncompletedByPersonIdAsync(BudgetId budgetId,
        PersonId participantId, CancellationToken cancellationToken)
    {
        return await _budgetSharingDbContext
            .BudgetPermissionRequests
            .Where(predicate: x => x.BudgetId == budgetId &&
                                   x.ParticipantId == participantId &&
                                   x.Status == BudgetPermissionRequestStatus.Pending)
            .ToListAsync(cancellationToken: cancellationToken);
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