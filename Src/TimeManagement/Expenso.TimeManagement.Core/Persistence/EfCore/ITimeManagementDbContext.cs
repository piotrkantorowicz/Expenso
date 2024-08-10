using Expenso.Shared.Database.EfCore.DbContexts;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;

using Microsoft.EntityFrameworkCore;

namespace Expenso.TimeManagement.Core.Persistence.EfCore;

internal interface ITimeManagementDbContext : IDbContext
{
    DbSet<JobEntry> JobEntries { get; }

    DbSet<JobInstance> JobInstances { get; }

    DbSet<JobEntryStatus> JobEntryStatuses { get; }

    EntityState GetEntryState(object entity);
}