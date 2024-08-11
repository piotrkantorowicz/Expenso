using Expenso.UserPreferences.Core.Domain.Preferences.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.UserPreferences.Core.Persistence.EfCore.Configurations;

internal sealed class GeneralPreferenceEntityTypeConfiguration : IEntityTypeConfiguration<GeneralPreference>
{
    public void Configure(EntityTypeBuilder<GeneralPreference> builder)
    {
        builder.ToTable(name: "GeneralPreferences");
        builder.HasKey(keyExpression: x => x.Id);
        builder.Property(propertyExpression: x => x.Id).IsRequired().ValueGeneratedNever();
        builder.Property(propertyExpression: x => x.UseDarkMode).IsRequired();
    }
}