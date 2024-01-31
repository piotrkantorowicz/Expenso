using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Model;

using Microsoft.EntityFrameworkCore;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore;

internal sealed class BudgetSharingDbContext(DbContextOptions<BudgetSharingDbContext> options)
    : DbContext(options), IBudgetSharingDbContext
{
    public DbSet<BudgetPermission> BudgetPermissions { get; set; } = null!;

    public DbSet<BudgetPermissionRequest> BudgetPermissionRequests { get; set; } = null!;

    public EntityState GetEntryState(object entity)
    {
        return Entry(entity).State;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("budget_sharing");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}