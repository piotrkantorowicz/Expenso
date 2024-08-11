using Expenso.UserPreferences.Core.Domain.Preferences.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.UserPreferences.Core.Persistence.EfCore.Configurations;

internal sealed class PreferenceEntityTypeConfiguration : IEntityTypeConfiguration<Preference>
{
    public void Configure(EntityTypeBuilder<Preference> builder)
    {
        builder.ToTable(name: "Preferences");
        builder.HasKey(keyExpression: x => x.Id);
        builder.Property(propertyExpression: x => x.Id).IsRequired().ValueGeneratedNever();
        builder.HasIndex(indexExpression: x => x.UserId).IsUnique();
        builder.Property(propertyExpression: x => x.UserId).IsRequired().ValueGeneratedNever();

        builder
            .HasOne(navigationExpression: x => x.FinancePreference)
            .WithOne()
            .HasForeignKey<FinancePreference>(foreignKeyExpression: x => x.PreferenceId);

        builder
            .HasOne(navigationExpression: x => x.NotificationPreference)
            .WithOne()
            .HasForeignKey<NotificationPreference>(foreignKeyExpression: x => x.PreferenceId);

        builder
            .HasOne(navigationExpression: x => x.GeneralPreference)
            .WithOne()
            .HasForeignKey<GeneralPreference>(foreignKeyExpression: x => x.PreferenceId);
    }
}