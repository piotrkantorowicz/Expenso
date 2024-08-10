using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Configurations;

internal sealed class BudgetPermissionRequestEntityTypeConfiguration : IEntityTypeConfiguration<BudgetPermissionRequest>
{
    public void Configure(EntityTypeBuilder<BudgetPermissionRequest> builder)
    {
        builder.ToTable(name: "BudgetPermissionRequests");
        builder.HasKey(keyExpression: x => x.Id);

        builder
            .Property(propertyExpression: x => x.Id)
            .HasConversion(convertToProviderExpression: x => x.Value,
                convertFromProviderExpression: x => BudgetPermissionRequestId.New(x))
            .IsRequired();

        builder
            .Property(propertyExpression: x => x.BudgetId)
            .HasConversion(convertToProviderExpression: x => x.Value,
                convertFromProviderExpression: x => BudgetId.New(x))
            .IsRequired();

        builder
            .Property(propertyExpression: x => x.ExpirationDate)
            .HasConversion(convertToProviderExpression: x => x == null ? (DateTimeOffset?)null : x.Value,
                convertFromProviderExpression: x => x.HasValue ? DateAndTime.New(x.Value) : null)
            .IsRequired(required: false);

        builder
            .Property(propertyExpression: x => x.ParticipantId)
            .HasConversion(convertToProviderExpression: x => x.Value,
                convertFromProviderExpression: x => PersonId.New(x))
            .IsRequired();

        builder
            .Property(propertyExpression: x => x.PermissionType)
            .HasConversion(convertToProviderExpression: x => x.Value,
                convertFromProviderExpression: x => PermissionType.Create(x))
            .IsRequired();

        builder
            .Property(propertyExpression: x => x.Status)
            .HasConversion(convertToProviderExpression: x => x.Value,
                convertFromProviderExpression: x => BudgetPermissionRequestStatus.Create(x))
            .HasColumnName(name: "status")
            .IsRequired();
    }
}