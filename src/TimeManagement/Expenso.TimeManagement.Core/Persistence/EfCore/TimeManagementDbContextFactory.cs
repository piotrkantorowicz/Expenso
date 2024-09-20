using Expenso.Shared.Database.EfCore.Npsql.DbContexts;

namespace Expenso.TimeManagement.Core.Persistence.EfCore;

internal sealed class TimeManagementDbContextFactory : NpsqlDbContextFactory<TimeManagementDbContext>;