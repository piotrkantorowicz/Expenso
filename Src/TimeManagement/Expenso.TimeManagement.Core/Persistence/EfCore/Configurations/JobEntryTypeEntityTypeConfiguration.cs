using Expenso.TimeManagement.Core.Domain.Jobs.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.TimeManagement.Core.Persistence.EfCore.Configurations;

internal sealed class JobEntryTypeEntityTypeConfiguration : IEntityTypeConfiguration<JobEntryType>
{
    public void Configure(EntityTypeBuilder<JobEntryType> builder)
    {
        builder.ToTable("JobEntryTypes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Code).IsRequired().HasMaxLength(5);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Interval).IsRequired();
    }
}