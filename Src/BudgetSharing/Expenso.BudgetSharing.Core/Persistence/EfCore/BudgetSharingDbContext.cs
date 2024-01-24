using Expenso.BudgetSharing.Core.Domain.BudgetPermissions.Model;

using Microsoft.EntityFrameworkCore;

namespace Expenso.BudgetSharing.Core.Persistence.EfCore;

internal sealed class BudgetSharingDbContext(DbContextOptions<BudgetSharingDbContext> options)
    : DbContext(options), IBudgetSharingDbContext
{
    public DbSet<BudgetPermission> BudgetPermissions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("budget_sharing");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}