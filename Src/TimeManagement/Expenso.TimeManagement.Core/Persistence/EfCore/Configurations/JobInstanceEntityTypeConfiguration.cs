using Expenso.TimeManagement.Core.Domain.Jobs.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.TimeManagement.Core.Persistence.EfCore.Configurations;

internal sealed class JobInstanceEntityTypeConfiguration : IEntityTypeConfiguration<JobInstance>
{
    public void Configure(EntityTypeBuilder<JobInstance> builder)
    {
        builder.ToTable("JobInstances");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.RunningDelay).IsRequired();
    }
}