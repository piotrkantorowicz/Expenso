using Expenso.TimeManagement.Core.Domain.Jobs.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.TimeManagement.Core.Persistence.EfCore.Configurations;

internal sealed class JobEntryStatusEntityTypeConfiguration : IEntityTypeConfiguration<JobEntryStatus>
{
    public void Configure(EntityTypeBuilder<JobEntryStatus> builder)
    {
        builder.ToTable("JobEntryStatuses");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Description).IsRequired(false).HasMaxLength(500);

        builder.HasData(JobEntryStatus.Running, JobEntryStatus.Completed, JobEntryStatus.Failed,
            JobEntryStatus.Retrying, JobEntryStatus.Cancelled);
    }
}