using Expenso.BudgetSharing.Domain.BudgetPermissions.Model;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Model.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Configurations;

internal sealed class BudgetPermissionEntityTypeConfiguration : IEntityTypeConfiguration<BudgetPermission>
{
    public void Configure(EntityTypeBuilder<BudgetPermission> builder)
    {
        builder.ToTable("budget_permissions");
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, x => BudgetPermissionId.Create(x))
            .HasColumnName("id")
            .IsRequired();

        builder.HasIndex(x => x.BudgetId).IsUnique();

        builder
            .Property(x => x.BudgetId)
            .HasConversion(x => x.Value, x => BudgetId.Create(x))
            .HasColumnName("budget_id")
            .IsRequired();

        builder
            .Property(x => x.OwnerId)
            .HasConversion(x => x.Value, x => PersonId.Create(x))
            .HasColumnName("accessor_id")
            .IsRequired();

        builder.OwnsMany(x => x.Permissions, permissionsBuilder =>
        {
            permissionsBuilder.ToTable("permissions");
            permissionsBuilder.HasKey(x => x.Id);

            permissionsBuilder
                .Property(x => x.Id)
                .HasConversion(x => x.Value, x => PermissionId.Create(x))
                .HasColumnName("id")
                .IsRequired();

            permissionsBuilder.WithOwner().HasForeignKey(x => x.BudgetPermissionId);

            permissionsBuilder
                .Property(x => x.BudgetPermissionId)
                .HasConversion(x => x.Value, x => BudgetPermissionId.Create(x))
                .HasColumnName("budget_permissions_id")
                .IsRequired();

            permissionsBuilder
                .Property(x => x.ParticipantId)
                .HasConversion(x => x.Value, x => PersonId.Create(x))
                .HasColumnName("participant_id")
                .IsRequired();

            permissionsBuilder.OwnsOne(x => x.PermissionType, permissionTypeBuilder =>
            {
                permissionTypeBuilder.Property(x => x.Value).HasColumnName("permission_type").IsRequired();
                permissionTypeBuilder.Ignore(x => x.DisplayName);
            });
        });
    }
}
