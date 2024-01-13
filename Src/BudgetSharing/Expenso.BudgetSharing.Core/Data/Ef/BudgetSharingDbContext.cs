using Expenso.BudgetSharing.Core.Models;

using Microsoft.EntityFrameworkCore;

namespace Expenso.BudgetSharing.Core.Data.Ef;

internal sealed class BudgetSharingDbContext(DbContextOptions<BudgetSharingDbContext> options)
    : DbContext(options), IBudgetSharingDbContext
{
    public DbSet<ShareBudget> ShareBudgets { get; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("budget_sharing");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}