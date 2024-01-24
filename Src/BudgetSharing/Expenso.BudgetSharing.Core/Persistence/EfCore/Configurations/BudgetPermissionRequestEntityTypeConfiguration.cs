using Expenso.BudgetSharing.Core.Domain.BudgetPermissionRequests.Model;
using Expenso.BudgetSharing.Core.Domain.BudgetPermissionRequests.Model.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.BudgetSharing.Core.Persistence.EfCore.Configurations;

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
    }
}