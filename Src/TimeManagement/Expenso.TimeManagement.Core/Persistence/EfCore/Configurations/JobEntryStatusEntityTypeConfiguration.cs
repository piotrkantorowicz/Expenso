using Expenso.TimeManagement.Core.Domain.Jobs.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.TimeManagement.Core.Persistence.EfCore.Configurations;

internal sealed class JobEntryStatusEntityTypeConfiguration : IEntityTypeConfiguration<JobEntryStatus>
{
    public void Configure(EntityTypeBuilder<JobEntryStatus> builder)
    {
        builder.ToTable(name: "JobEntryStatuses");
        builder.HasKey(keyExpression: x => x.Id);
        builder.Property(propertyExpression: x => x.Id).IsRequired();
        builder.Property(propertyExpression: x => x.Name).IsRequired().HasMaxLength(maxLength: 150);

        builder
            .Property(propertyExpression: x => x.Description)
            .IsRequired(required: false)
            .HasMaxLength(maxLength: 500);
    }
}