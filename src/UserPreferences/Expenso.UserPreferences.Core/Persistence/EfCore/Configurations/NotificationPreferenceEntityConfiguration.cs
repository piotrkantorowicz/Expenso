using Expenso.UserPreferences.Core.Domain.Preferences.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenso.UserPreferences.Core.Persistence.EfCore.Configurations;

internal sealed class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<NotificationPreference>
{
    public void Configure(EntityTypeBuilder<NotificationPreference> builder)
    {
        builder.ToTable(name: "NotificationPreferences");
        builder.HasKey(keyExpression: x => x.Id);
        builder.Property(propertyExpression: x => x.Id).IsRequired().ValueGeneratedNever();
        builder.Property(propertyExpression: x => x.SendFinanceReportEnabled).IsRequired();
        builder.Property(propertyExpression: x => x.SendFinanceReportInterval).IsRequired();
    }
}