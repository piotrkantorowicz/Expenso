namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Configurations;

// internal sealed class PermissionEntityTypeConfiguration : IEntityTypeConfiguration<Permission>
// {
//     public void Configure(EntityTypeBuilder<Permission> builder)
//     {
//         builder.ToTable("permissions");
//
//         builder
//             .Property(x => x.BudgetPermissionId)
//             .HasConversion(x => x.Value, x => BudgetPermissionId.Create(x))
//             .HasColumnName("budget_permission_id")
//             .IsRequired();
//
//         builder
//             .Property(x => x.ParticipantId)
//             .HasConversion(x => x.Value, x => PersonId.Create(x))
//             .HasColumnName("participant_id")
//             .IsRequired();
//
//         builder
//             .Property(x => x.PermissionType)
//             .HasConversion(x => x.Value, x => PermissionType.Create(x))
//             .HasColumnName("permission_type")
//             .IsRequired();
//     }
// }