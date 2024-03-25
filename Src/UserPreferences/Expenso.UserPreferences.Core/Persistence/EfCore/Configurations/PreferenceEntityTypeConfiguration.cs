using Expenso.UserPreferences.Core.Domain.Preferences.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.UserPreferences.Core.Persistence.EfCore.Configurations;

internal sealed class PreferenceEntityTypeConfiguration : IEntityTypeConfiguration<Preference>
{
    public void Configure(EntityTypeBuilder<Preference> builder)
    {
        builder.ToTable("Preferences");
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