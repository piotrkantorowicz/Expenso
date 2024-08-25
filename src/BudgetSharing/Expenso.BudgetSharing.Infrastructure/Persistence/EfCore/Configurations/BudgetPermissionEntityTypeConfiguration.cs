using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Configurations;

internal sealed class BudgetPermissionEntityTypeConfiguration : IEntityTypeConfiguration<BudgetPermission>
{
    public void Configure(EntityTypeBuilder<BudgetPermission> builder)
    {
        builder.ToTable(name: "BudgetPermissions");
        builder.HasKey(keyExpression: x => x.Id);

        builder
            .Property(propertyExpression: x => x.Id)
            .HasConversion(convertToProviderExpression: x => x.Value,
                convertFromProviderExpression: x => BudgetPermissionId.New(x))
            .IsRequired();

        builder.HasIndex(indexExpression: x => x.BudgetId).IsUnique();

        builder
            .Property(propertyExpression: x => x.BudgetId)
            .HasConversion(convertToProviderExpression: x => x.Value,
                convertFromProviderExpression: x => BudgetId.New(x))
            .IsRequired();

        builder
            .Property(propertyExpression: x => x.OwnerId)
            .HasConversion(convertToProviderExpression: x => x.Value,
                convertFromProviderExpression: x => PersonId.New(x))
            .IsRequired();

        builder.OwnsOne<Blocker>(navigationExpression: x => x.Blocker, buildAction: removalBuilder =>
        {
            removalBuilder.Property(propertyExpression: x => x.IsBlocked);
            removalBuilder.Property(propertyExpression: x => x.BlockDate);
        });

        builder.OwnsMany(navigationExpression: x => x.Permissions, buildAction: permissionsBuilder =>
        {
            permissionsBuilder.ToTable(name: "Permissions");

            permissionsBuilder
                .Property(propertyExpression: x => x.ParticipantId)
                .HasConversion(convertToProviderExpression: x => x.Value,
                    convertFromProviderExpression: x => PersonId.New(x))
                .IsRequired();

            permissionsBuilder
                .Property(propertyExpression: x => x.PermissionType)
                .HasConversion(convertToProviderExpression: x => x.Value,
                    convertFromProviderExpression: x => PermissionType.Create(x))
                .IsRequired();
        });

        // TODO: Investigate what is wrong with that filter
        // builder.HasQueryFilter(filter: x => x.Deletion == null || x.Deletion.IsDeleted is false);
    }
}