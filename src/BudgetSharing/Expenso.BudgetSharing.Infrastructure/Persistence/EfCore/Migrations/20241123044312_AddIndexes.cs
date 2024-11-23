using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Migrations;

/// <inheritdoc />
public partial class AddIndexes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "BudgetCode",
            schema: "BudgetSharing",
            table: "BudgetPermissions",
            type: "character varying(18)",
            maxLength: 18,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.CreateIndex(
            name: "IX_BudgetPermissions_BudgetCode",
            schema: "BudgetSharing",
            table: "BudgetPermissions",
            column: "BudgetCode");

        migrationBuilder.CreateIndex(
            name: "IX_BudgetPermissions_OwnerId",
            schema: "BudgetSharing",
            table: "BudgetPermissions",
            column: "OwnerId");

        migrationBuilder.CreateIndex(
            name: "IX_BudgetPermissionRequests_BudgetCode",
            schema: "BudgetSharing",
            table: "BudgetPermissionRequests",
            column: "BudgetCode");

        migrationBuilder.CreateIndex(
            name: "IX_BudgetPermissionRequests_BudgetId",
            schema: "BudgetSharing",
            table: "BudgetPermissionRequests",
            column: "BudgetId");

        migrationBuilder.CreateIndex(
            name: "IX_BudgetPermissionRequests_ParticipantId",
            schema: "BudgetSharing",
            table: "BudgetPermissionRequests",
            column: "ParticipantId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_BudgetPermissions_BudgetCode",
            schema: "BudgetSharing",
            table: "BudgetPermissions");

        migrationBuilder.DropIndex(
            name: "IX_BudgetPermissions_OwnerId",
            schema: "BudgetSharing",
            table: "BudgetPermissions");

        migrationBuilder.DropIndex(
            name: "IX_BudgetPermissionRequests_BudgetCode",
            schema: "BudgetSharing",
            table: "BudgetPermissionRequests");

        migrationBuilder.DropIndex(
            name: "IX_BudgetPermissionRequests_BudgetId",
            schema: "BudgetSharing",
            table: "BudgetPermissionRequests");

        migrationBuilder.DropIndex(
            name: "IX_BudgetPermissionRequests_ParticipantId",
            schema: "BudgetSharing",
            table: "BudgetPermissionRequests");

        migrationBuilder.AlterColumn<string>(
            name: "BudgetCode",
            schema: "BudgetSharing",
            table: "BudgetPermissions",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(18)",
            oldMaxLength: 18);
    }
}