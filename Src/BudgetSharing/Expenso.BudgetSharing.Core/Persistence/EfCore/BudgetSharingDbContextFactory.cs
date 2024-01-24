using Expenso.Shared.Database.EfCore.NpSql.DbContexts;

namespace Expenso.BudgetSharing.Core.Persistence.EfCore;

internal sealed class BudgetSharingDbContextFactory : NpsqlDbContextFactory<BudgetSharingDbContext>;