using Expenso.TimeManagement.Core.Domain.Jobs.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.TimeManagement.Core.Persistence.EfCore.Configurations;

internal sealed class JobEntryEntityTypeConfiguration : IEntityTypeConfiguration<JobEntry>
{
    public void Configure(EntityTypeBuilder<JobEntry> builder)
    {
        builder.ToTable(name: "JobEntries");
        builder.HasKey(keyExpression: x => x.Id);
        builder.Property(propertyExpression: x => x.Id).IsRequired().ValueGeneratedNever();

        builder
            .HasOne(navigationExpression: e => e.JobInstance)
            .WithMany()
            .HasForeignKey(foreignKeyExpression: e => e.JobInstanceId)
            .IsRequired();

        builder.Property(propertyExpression: x => x.CronExpression).IsRequired(required: false);
        builder.Property(propertyExpression: x => x.RunAt).IsRequired(required: false);
        builder.Property(propertyExpression: x => x.IsCompleted).IsRequired(required: false);
        builder.Property(propertyExpression: x => x.CurrentRetries).IsRequired(required: false);
        builder.Property(propertyExpression: x => x.MaxRetries).IsRequired();
        builder.Property(propertyExpression: x => x.LastRun).IsRequired(required: false);

        builder
            .HasOne(navigationExpression: e => e.JobStatus)
            .WithMany()
            .HasForeignKey(foreignKeyExpression: e => e.JobEntryStatusId)
            .IsRequired();

        builder.OwnsMany(navigationExpression: x => x.Triggers, buildAction: jobEntryTriggersBuilder =>
        {
            jobEntryTriggersBuilder.ToTable(name: "JobEntryTriggers");
            jobEntryTriggersBuilder.HasKey(keyExpression: x => x.Id);
            jobEntryTriggersBuilder.Property(propertyExpression: x => x.Id).IsRequired().ValueGeneratedNever();

            jobEntryTriggersBuilder
                .Property(propertyExpression: x => x.EventType)
                .IsRequired()
                .HasMaxLength(maxLength: 500);

            jobEntryTriggersBuilder.Property(propertyExpression: x => x.EventData).IsRequired();
        });
    }
}