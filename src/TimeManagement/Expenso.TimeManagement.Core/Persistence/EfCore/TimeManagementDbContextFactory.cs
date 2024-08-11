using Expenso.Shared.Database.EfCore.NpSql.DbContexts;

namespace Expenso.TimeManagement.Core.Persistence.EfCore;

internal sealed class TimeManagementDbContextFactory : NpsqlDbContextFactory<TimeManagementDbContext>;