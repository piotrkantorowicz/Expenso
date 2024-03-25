using Expenso.Shared.Database.EfCore.NpSql.DbContexts;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;

using Microsoft.EntityFrameworkCore;

namespace Expenso.TimeManagement.Core.Persistence.EfCore;

internal interface ITimeManagementDbContext : IDbContext
{
    DbSet<JobEntry> JobEntries { get; }

    DbSet<JobEntryType> JobEntryTypes { get; }

    DbSet<JobEntryStatus> JobEntryStatuses { get; }

    EntityState GetEntryState(object entity);
}