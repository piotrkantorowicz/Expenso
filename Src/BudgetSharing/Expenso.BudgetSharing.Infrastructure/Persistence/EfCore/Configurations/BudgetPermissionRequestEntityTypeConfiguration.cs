using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Domain.Types.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Configurations;

internal sealed class BudgetPermissionRequestEntityTypeConfiguration : IEntityTypeConfiguration<BudgetPermissionRequest>
{
    public void Configure(EntityTypeBuilder<BudgetPermissionRequest> builder)
    {
        builder.ToTable("BudgetPermissions_Requests");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => BudgetPermissionRequestId.New(x)).IsRequired();
        builder.Property(x => x.BudgetId).HasConversion(x => x.Value, x => BudgetId.New(x)).IsRequired();

        builder
            .Property(x => x.ExpirationDate)
            .HasConversion(x => x == null ? (DateTimeOffset?)null : x.Value,
                x => x.HasValue ? DateAndTime.New(x.Value) : null)
            .IsRequired(false);

        builder.Property(x => x.ParticipantId).HasConversion(x => x.Value, x => PersonId.New(x)).IsRequired();
        builder.Property(x => x.PermissionType).HasConversion(x => x.Value, x => PermissionType.Create(x)).IsRequired();

        builder
            .Property(x => x.Status)
            .HasConversion(x => x.Value, x => BudgetPermissionRequestStatus.Create(x))
            .HasColumnName("status")
            .IsRequired();
    }
}