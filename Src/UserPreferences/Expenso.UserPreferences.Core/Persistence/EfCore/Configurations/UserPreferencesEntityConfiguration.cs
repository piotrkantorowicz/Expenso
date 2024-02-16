using Expenso.UserPreferences.Core.Domain.Preferences.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.UserPreferences.Core.Persistence.EfCore.Configurations;

internal sealed class UserPreferencesEntityConfiguration : IEntityTypeConfiguration<Preference>,
    IEntityTypeConfiguration<FinancePreference>, IEntityTypeConfiguration<NotificationPreference>,
    IEntityTypeConfiguration<GeneralPreference>
{
    public void Configure(EntityTypeBuilder<FinancePreference> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
        builder.Property(x => x.AllowAddFinancePlanReviewers).IsRequired();
        builder.Property(x => x.AllowAddFinancePlanSubOwners).IsRequired();
        builder.Property(x => x.MaxNumberOfFinancePlanReviewers).IsRequired();
        builder.Property(x => x.MaxNumberOfSubFinancePlanSubOwners).IsRequired();
    }

    public void Configure(EntityTypeBuilder<GeneralPreference> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
        builder.Property(x => x.UseDarkMode).IsRequired();
    }

    public void Configure(EntityTypeBuilder<NotificationPreference> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
        builder.Property(x => x.SendFinanceReportEnabled).IsRequired();
        builder.Property(x => x.SendFinanceReportInterval).IsRequired();
    }

    public void Configure(EntityTypeBuilder<Preference> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
        builder.HasIndex(x => x.UserId).IsUnique();
        builder.Property(x => x.UserId).IsRequired().ValueGeneratedNever();
        builder.HasOne(x => x.FinancePreference).WithOne().HasForeignKey<FinancePreference>(x => x.PreferenceId);

        builder
            .HasOne(x => x.NotificationPreference)
            .WithOne()
            .HasForeignKey<NotificationPreference>(x => x.PreferenceId);

        builder.HasOne(x => x.GeneralPreference).WithOne().HasForeignKey<GeneralPreference>(x => x.PreferenceId);
    }
}