using Expenso.BudgetSharing.Core.Models;

using Microsoft.EntityFrameworkCore;

namespace Expenso.BudgetSharing.Core.Data.Ef;

internal interface IBudgetSharingDbContext
{
    DbSet<ShareBudget> ShareBudgets { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}