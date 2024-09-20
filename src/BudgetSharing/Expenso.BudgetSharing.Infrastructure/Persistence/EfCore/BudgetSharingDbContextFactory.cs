using Expenso.Shared.Database.EfCore.Npsql.DbContexts;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore;

internal sealed class BudgetSharingDbContextFactory : NpsqlDbContextFactory<BudgetSharingDbContext>;