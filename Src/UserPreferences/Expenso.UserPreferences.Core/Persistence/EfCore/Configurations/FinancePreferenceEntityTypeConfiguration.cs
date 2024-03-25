using Expenso.UserPreferences.Core.Domain.Preferences.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.UserPreferences.Core.Persistence.EfCore.Configurations;

internal sealed class FinancePreferenceEntityTypeConfiguration : IEntityTypeConfiguration<FinancePreference>
{
    public void Configure(EntityTypeBuilder<FinancePreference> builder)
    {
        builder.ToTable("FinancePreferences");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
        builder.Property(x => x.AllowAddFinancePlanReviewers).IsRequired();
        builder.Property(x => x.AllowAddFinancePlanSubOwners).IsRequired();
        builder.Property(x => x.MaxNumberOfFinancePlanReviewers).IsRequired();
        builder.Property(x => x.MaxNumberOfSubFinancePlanSubOwners).IsRequired();
    }
}