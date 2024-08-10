using Expenso.UserPreferences.Core.Domain.Preferences.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.UserPreferences.Core.Persistence.EfCore.Configurations;

internal sealed class GeneralPreferenceEntityTypeConfiguration : IEntityTypeConfiguration<GeneralPreference>
{
    public void Configure(EntityTypeBuilder<GeneralPreference> builder)
    {
        builder.ToTable("GeneralPreferences");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
        builder.Property(x => x.UseDarkMode).IsRequired();
    }
}