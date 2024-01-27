using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared;
using Expenso.Shared.Domain.Types.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Configurations;

internal sealed class BudgetPermissionRequestEntityTypeConfiguration : IEntityTypeConfiguration<BudgetPermissionRequest>
{
    public void Configure(EntityTypeBuilder<BudgetPermissionRequest> builder)
    {
        builder.ToTable("budget_permission_requests");
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, x => BudgetPermissionRequestId.Create(x))
            .HasColumnName("id")
            .IsRequired();

        builder
            .Property(x => x.BudgetId)
            .HasConversion(x => x.Value, x => BudgetId.Create(x))
            .HasColumnName("budget_id")
            .IsRequired();

        builder
            .Property(x => x.ExpirationDate)
            .HasConversion(x => x == null ? (DateTimeOffset?)null : x.Value,
                x => x.HasValue ? DateAndTime.Create(x.Value) : null)
            .HasColumnName("expiration_date")
            .IsRequired(false);

        builder
            .Property(x => x.ParticipantId)
            .HasConversion(x => x.Value, x => PersonId.Create(x))
            .HasColumnName("participant_id")
            .IsRequired();

        builder.OwnsOne(x => x.PermissionType, permissionTypeBuilder =>
        {
            permissionTypeBuilder.Property(x => x.Value).HasColumnName("permission_type").IsRequired();
            permissionTypeBuilder.Ignore(x => x.DisplayName);
        });

        builder.OwnsOne(x => x.Status, statusTypeBuilder =>
        {
            statusTypeBuilder.Property(x => x.Value).HasColumnName("status").IsRequired();
            statusTypeBuilder.Ignore(x => x.DisplayName);
        });
    }
}
