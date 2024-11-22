using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Migrations;

/// <inheritdoc />
public partial class AddUniqueConstraintForOwnerIdAndCode : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_BudgetPermissions_OwnerId_BudgetCode",
            schema: "BudgetSharing",
            table: "BudgetPermissions",
            columns: new[] { "OwnerId", "BudgetCode" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_BudgetPermissions_OwnerId_BudgetCode",
            schema: "BudgetSharing",
            table: "BudgetPermissions");
    }
}