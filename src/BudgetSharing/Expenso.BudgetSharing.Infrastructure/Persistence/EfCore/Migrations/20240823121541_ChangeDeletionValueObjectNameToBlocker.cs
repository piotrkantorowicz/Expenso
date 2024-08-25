using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDeletionValueObjectNameToBlocker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Deletion_RemovalDate",
                schema: "BudgetSharing",
                table: "BudgetPermissions",
                newName: "Blocker_BlockDate");

            migrationBuilder.RenameColumn(
                name: "Deletion_IsDeleted",
                schema: "BudgetSharing",
                table: "BudgetPermissions",
                newName: "Blocker_IsBlocked");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Blocker_IsBlocked",
                schema: "BudgetSharing",
                table: "BudgetPermissions",
                newName: "Deletion_IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Blocker_BlockDate",
                schema: "BudgetSharing",
                table: "BudgetPermissions",
                newName: "Deletion_RemovalDate");
        }
    }
}
