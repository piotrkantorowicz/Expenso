using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Migrations;

/// <inheritdoc />
public partial class AddOwnerIdColumnInBudgetPermissionRequestsTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "OwnerId",
            schema: "BudgetSharing",
            table: "BudgetPermissionRequests",
            type: "uuid",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.CreateIndex(
            name: "IX_BudgetPermissionRequests_OwnerId",
            schema: "BudgetSharing",
            table: "BudgetPermissionRequests",
            column: "OwnerId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_BudgetPermissionRequests_OwnerId",
            schema: "BudgetSharing",
            table: "BudgetPermissionRequests");

        migrationBuilder.DropColumn(
            name: "OwnerId",
            schema: "BudgetSharing",
            table: "BudgetPermissionRequests");
    }
}