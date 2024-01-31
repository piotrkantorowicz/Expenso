using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Model;

using Microsoft.EntityFrameworkCore;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore;

internal interface IBudgetSharingDbContext
{
    DbSet<BudgetPermission> BudgetPermissions { get; }

    DbSet<BudgetPermissionRequest> BudgetPermissionRequests { get; }

    EntityState GetEntryState(object entity);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}