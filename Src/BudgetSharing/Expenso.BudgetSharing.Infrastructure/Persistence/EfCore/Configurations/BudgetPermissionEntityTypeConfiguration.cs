using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Configurations;

internal sealed class BudgetPermissionEntityTypeConfiguration : IEntityTypeConfiguration<BudgetPermission>
{
    public void Configure(EntityTypeBuilder<BudgetPermission> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => BudgetPermissionId.New(x)).IsRequired();
        builder.HasIndex(x => x.BudgetId).IsUnique();
        builder.Property(x => x.BudgetId).HasConversion(x => x.Value, x => BudgetId.New(x)).IsRequired();
        builder.Property(x => x.OwnerId).HasConversion(x => x.Value, x => PersonId.New(x)).IsRequired();

        builder.OwnsMany(x => x.Permissions, permissionsBuilder =>
        {
            permissionsBuilder
                .Property(x => x.ParticipantId)
                .HasConversion(x => x.Value, x => PersonId.New(x))
                .IsRequired();

            permissionsBuilder
                .Property(x => x.PermissionType)
                .HasConversion(x => x.Value, x => PermissionType.Create(x))
                .IsRequired();
        });
    }
}