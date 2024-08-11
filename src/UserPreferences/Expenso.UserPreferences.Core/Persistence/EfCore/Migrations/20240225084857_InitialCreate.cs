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
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancePreferences",
                schema: "UserPreferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PreferenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaxNumberOfFinancePlanReviewers = table.Column<int>(type: "integer", nullable: false),
                    AllowAddFinancePlanReviewers = table.Column<bool>(type: "boolean", nullable: false),
                    MaxNumberOfSubFinancePlanSubOwners = table.Column<int>(type: "integer", nullable: false),
                    AllowAddFinancePlanSubOwners = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancePreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancePreferences_Preferences_PreferenceId",
                        column: x => x.PreferenceId,
                        principalSchema: "UserPreferences",
                        principalTable: "Preferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneralPreferences",
                schema: "UserPreferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PreferenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    UseDarkMode = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralPreferences_Preferences_PreferenceId",
                        column: x => x.PreferenceId,
                        principalSchema: "UserPreferences",
                        principalTable: "Preferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationPreferences",
                schema: "UserPreferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PreferenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    SendFinanceReportInterval = table.Column<int>(type: "integer", nullable: false),
                    SendFinanceReportEnabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationPreferences_Preferences_PreferenceId",
                        column: x => x.PreferenceId,
                        principalSchema: "UserPreferences",
                        principalTable: "Preferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancePreferences_PreferenceId",
                schema: "UserPreferences",
                table: "FinancePreferences",
                column: "PreferenceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralPreferences_PreferenceId",
                schema: "UserPreferences",
                table: "GeneralPreferences",
                column: "PreferenceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationPreferences_PreferenceId",
                schema: "UserPreferences",
                table: "NotificationPreferences",
                column: "PreferenceId",
                unique: true);

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
                name: "FinancePreferences",
                schema: "UserPreferences");

            migrationBuilder.DropTable(
                name: "GeneralPreferences",
                schema: "UserPreferences");

            migrationBuilder.DropTable(
                name: "NotificationPreferences",
                schema: "UserPreferences");

            migrationBuilder.DropTable(
                name: "Preferences",
                schema: "UserPreferences");
        }
    }
}
