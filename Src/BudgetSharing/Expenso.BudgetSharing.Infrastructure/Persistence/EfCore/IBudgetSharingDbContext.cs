using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.Shared.Database.EfCore.DbContexts;
using Expenso.Shared.Database.EfCore.Migrations;
using Expenso.Shared.Domain.Types.Events;

using Microsoft.EntityFrameworkCore;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore;

public interface IBudgetSharingDbContext : IDbContext, IDoNotSeed
{
    DbSet<BudgetPermission> BudgetPermissions { get; }

    DbSet<BudgetPermissionRequest> BudgetPermissionRequests { get; }

    EntityState GetEntryState(object entity);

    Task BeginTransactionAsync(CancellationToken cancellationToken);

    Task RollbackTransactionAsync(CancellationToken cancellationToken);

    Task CommitTransactionAsync(CancellationToken cancellationToken);

    IReadOnlyCollection<IDomainEvent> GetUncommittedChanges();
}