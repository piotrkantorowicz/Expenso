using Expenso.BudgetSharing.Core.Domain.BudgetPermissions.Model;

using Microsoft.EntityFrameworkCore;

namespace Expenso.BudgetSharing.Core.Persistence.EfCore;

internal interface IBudgetSharingDbContext
{
    DbSet<BudgetPermission> BudgetPermissions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}