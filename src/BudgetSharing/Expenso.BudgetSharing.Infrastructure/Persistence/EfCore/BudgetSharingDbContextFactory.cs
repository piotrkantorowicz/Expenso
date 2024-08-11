using Expenso.Shared.Database.EfCore.NpSql.DbContexts;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore;

internal sealed class BudgetSharingDbContextFactory : NpsqlDbContextFactory<BudgetSharingDbContext>;