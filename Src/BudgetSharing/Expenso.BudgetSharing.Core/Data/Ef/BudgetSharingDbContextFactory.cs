using Expenso.Shared.Database.EfCore.NpSql.DbContexts;

namespace Expenso.BudgetSharing.Core.Data.Ef;

internal sealed class BudgetSharingDbContextFactory : NpsqlDbContextFactory<BudgetSharingDbContext>;