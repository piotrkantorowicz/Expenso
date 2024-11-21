using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintForBudgetIdAndCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BudgetPermissions_BudgetId_BudgetCode",
                schema: "BudgetSharing",
                table: "BudgetPermissions",
                columns: new[] { "BudgetId", "BudgetCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BudgetPermissions_BudgetId_BudgetCode",
                schema: "BudgetSharing",
                table: "BudgetPermissions");
        }
    }
}
