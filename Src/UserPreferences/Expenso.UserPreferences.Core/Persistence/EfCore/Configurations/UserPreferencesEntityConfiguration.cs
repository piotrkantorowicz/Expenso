using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.UserPreferences.Core.Persistence.EfCore.Configurations;

internal sealed class UserPreferencesEntityConfiguration : IEntityTypeConfiguration<Preference>
{
    public void Configure(EntityTypeBuilder<Preference> builder)
    {
        builder.ToTable("preferences");

        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, x => PreferenceId.Create(x))
            .HasColumnName("id")
            .IsRequired();

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.UserId)
            .HasConversion(x => x.Value, x => UserId.Create(x))
            .HasColumnName("user_id")
            .IsRequired();

        builder.HasIndex(x => x.UserId).IsUnique();

        builder.OwnsOne(x => x.FinancePreference, financePreferencesBuilder =>
        {
            financePreferencesBuilder
                .Property(x => x.AllowAddFinancePlanReviewers)
                .HasColumnName("allow_add_finance_plan_reviewers")
                .IsRequired();

            financePreferencesBuilder
                .Property(x => x.AllowAddFinancePlanSubOwners)
                .HasColumnName("allow_add_finance_plan_sub_owners")
                .IsRequired();

            financePreferencesBuilder
                .Property(x => x.MaxNumberOfFinancePlanReviewers)
                .HasColumnName("max_number_of_finance_plan_reviewers")
                .IsRequired();

            financePreferencesBuilder
                .Property(x => x.MaxNumberOfSubFinancePlanSubOwners)
                .HasColumnName("max_number_of_sub_finance_plan_sub_owners")
                .IsRequired();
        });

        builder.OwnsOne(x => x.NotificationPreference, notificationPreferencesBuilder =>
        {
            notificationPreferencesBuilder
                .Property(x => x.SendFinanceReportEnabled)
                .HasColumnName("send_finance_report_enabled")
                .IsRequired();

            notificationPreferencesBuilder
                .Property(x => x.SendFinanceReportInterval)
                .HasColumnName("send_finance_report_interval")
                .IsRequired();
        });

        builder.OwnsOne(x => x.GeneralPreference, generalPreferencesBuilder =>
        {
            generalPreferencesBuilder.Property(x => x.UseDarkMode).HasColumnName("use_dark_mode").IsRequired();
        });
    }
}