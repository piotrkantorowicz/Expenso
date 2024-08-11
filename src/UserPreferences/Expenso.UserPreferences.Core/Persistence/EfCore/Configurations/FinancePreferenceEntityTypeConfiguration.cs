using Expenso.UserPreferences.Core.Domain.Preferences.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.UserPreferences.Core.Persistence.EfCore.Configurations;

internal sealed class FinancePreferenceEntityTypeConfiguration : IEntityTypeConfiguration<FinancePreference>
{
    public void Configure(EntityTypeBuilder<FinancePreference> builder)
    {
        builder.ToTable(name: "FinancePreferences");
        builder.HasKey(keyExpression: x => x.Id);
        builder.Property(propertyExpression: x => x.Id).IsRequired().ValueGeneratedNever();
        builder.Property(propertyExpression: x => x.AllowAddFinancePlanReviewers).IsRequired();
        builder.Property(propertyExpression: x => x.AllowAddFinancePlanSubOwners).IsRequired();
        builder.Property(propertyExpression: x => x.MaxNumberOfFinancePlanReviewers).IsRequired();
        builder.Property(propertyExpression: x => x.MaxNumberOfSubFinancePlanSubOwners).IsRequired();
    }
}