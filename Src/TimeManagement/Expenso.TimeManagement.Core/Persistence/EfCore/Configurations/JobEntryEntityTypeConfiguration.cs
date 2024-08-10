using Expenso.TimeManagement.Core.Domain.Jobs.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.TimeManagement.Core.Persistence.EfCore.Configurations;

internal sealed class JobEntryEntityTypeConfiguration : IEntityTypeConfiguration<JobEntry>
{
    public void Configure(EntityTypeBuilder<JobEntry> builder)
    {
        builder.ToTable("JobEntries");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
        builder.HasOne(e => e.JobInstance).WithMany().HasForeignKey(e => e.JobInstanceId).IsRequired();
        
        builder.Property(x => x.CronExpression).IsRequired();
        builder.Property(x => x.IsCompleted).IsRequired(false);
        builder.Property(x => x.CurrentRetries).IsRequired(false);
        builder.Property(x => x.MaxRetries).IsRequired();
        builder.Property(x => x.LastRun).IsRequired(false);
        
        builder
            .HasOne(e => e.JobStatus)
            .WithMany()
            .HasForeignKey(e => e.JobEntryStatusId)
            .IsRequired();
        
        builder.OwnsMany(x => x.Triggers, jobEntryTriggersBuilder =>
        {
            jobEntryTriggersBuilder.ToTable("JobEntryTriggers");
            jobEntryTriggersBuilder.HasKey(x => x.Id);
            jobEntryTriggersBuilder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            jobEntryTriggersBuilder.Property(x => x.EventType).IsRequired().HasMaxLength(150);
            jobEntryTriggersBuilder.Property(x => x.EventData).IsRequired();
        });
    }
}