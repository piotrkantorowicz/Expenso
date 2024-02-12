using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.UserPreferences.Core.Persistence.EfCore.Configurations;

internal sealed class UserPreferencesEntityConfiguration : IEntityTypeConfiguration<Preference>
{
    public void Configure(EntityTypeBuilder<Preference> builder)
    {
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => PreferenceId.New(x)).IsRequired();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).HasConversion(x => x.Value, x => UserId.New(x)).IsRequired();
        builder.HasIndex(x => x.UserId).IsUnique();

        builder.OwnsOne(x => x.FinancePreference, financePreferencesBuilder =>
        {
            financePreferencesBuilder
                .Property(x => x.AllowAddFinancePlanReviewers)
                .HasColumnName("AllowAddFinancePlanReviewers")
                .IsRequired();

            financePreferencesBuilder
                .Property(x => x.AllowAddFinancePlanSubOwners)
                .HasColumnName("AllowAddFinancePlanSubOwners")
                .IsRequired();

            financePreferencesBuilder
                .Property(x => x.MaxNumberOfFinancePlanReviewers)
                .HasColumnName("MaxNumberOfFinancePlanReviewers")
                .IsRequired();

            financePreferencesBuilder
                .Property(x => x.MaxNumberOfSubFinancePlanSubOwners)
                .HasColumnName("MaxNumberOfSubFinancePlanSubOwners")
                .IsRequired();
        });

        builder.OwnsOne(x => x.NotificationPreference, notificationPreferencesBuilder =>
        {
            notificationPreferencesBuilder
                .Property(x => x.SendFinanceReportEnabled)
                .HasColumnName("SendFinanceReportEnabled")
                .IsRequired();

            notificationPreferencesBuilder
                .Property(x => x.SendFinanceReportInterval)
                .HasColumnName("SendFinanceReportInterval")
                .IsRequired();
        });

        builder.OwnsOne(x => x.GeneralPreference, generalPreferencesBuilder =>
        {
            generalPreferencesBuilder.Property(x => x.UseDarkMode).IsRequired();
        });
    }
}