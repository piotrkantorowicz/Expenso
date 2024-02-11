using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expenso.UserPreferences.Core.Persistence.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "UserPreferences");

            migrationBuilder.CreateTable(
                name: "Preferences",
                schema: "UserPreferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    GeneralPreference_UseDarkMode = table.Column<bool>(type: "boolean", nullable: true),
                    MaxNumberOfFinancePlanReviewers = table.Column<int>(type: "integer", nullable: true),
                    AllowAddFinancePlanReviewers = table.Column<bool>(type: "boolean", nullable: true),
                    MaxNumberOfSubFinancePlanSubOwners = table.Column<int>(type: "integer", nullable: true),
                    AllowAddFinancePlanSubOwners = table.Column<bool>(type: "boolean", nullable: true),
                    SendFinanceReportInterval = table.Column<int>(type: "integer", nullable: true),
                    SendFinanceReportEnabled = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preferences", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_UserId",
                schema: "UserPreferences",
                table: "Preferences",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Preferences",
                schema: "UserPreferences");
        }
    }
}
