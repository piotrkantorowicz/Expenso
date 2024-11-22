using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Migrations;

/// <inheritdoc />
public partial class AddBudgetCode : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "BudgetCode",
            schema: "BudgetSharing",
            table: "BudgetPermissions",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "BudgetCode",
            schema: "BudgetSharing",
            table: "BudgetPermissionRequests",
            type: "text",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "BudgetCode",
            schema: "BudgetSharing",
            table: "BudgetPermissions");

        migrationBuilder.DropColumn(
            name: "BudgetCode",
            schema: "BudgetSharing",
            table: "BudgetPermissionRequests");
    }
}