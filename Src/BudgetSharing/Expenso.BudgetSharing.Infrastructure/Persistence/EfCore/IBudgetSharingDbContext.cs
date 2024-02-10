using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.Shared.Database.EfCore.NpSql.DbContexts;
using Expenso.Shared.Domain.Types.Events;

using Microsoft.EntityFrameworkCore;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore;

public interface IBudgetSharingDbContext : IDbContext
{
    DbSet<BudgetPermission> BudgetPermissions { get; }

    DbSet<BudgetPermissionRequest> BudgetPermissionRequests { get; }

    EntityState GetEntryState(object entity);

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    IReadOnlyCollection<IDomainEvent> GetUncommittedChanges();
}