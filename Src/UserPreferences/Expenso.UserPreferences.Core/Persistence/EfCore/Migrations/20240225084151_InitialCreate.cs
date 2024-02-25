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
                name: "Preferences_FinancePreferences",
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
                    table.PrimaryKey("PK_Preferences_FinancePreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Preferences_FinancePreferences_Preferences_PreferenceId",
                        column: x => x.PreferenceId,
                        principalSchema: "UserPreferences",
                        principalTable: "Preferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Preferences_GeneralPreferences",
                schema: "UserPreferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PreferenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    UseDarkMode = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preferences_GeneralPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Preferences_GeneralPreferences_Preferences_PreferenceId",
                        column: x => x.PreferenceId,
                        principalSchema: "UserPreferences",
                        principalTable: "Preferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Preferences_NotificationPreferences",
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
                    table.PrimaryKey("PK_Preferences_NotificationPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Preferences_NotificationPreferences_Preferences_PreferenceId",
                        column: x => x.PreferenceId,
                        principalSchema: "UserPreferences",
                        principalTable: "Preferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_UserId",
                schema: "UserPreferences",
                table: "Preferences",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_FinancePreferences_PreferenceId",
                schema: "UserPreferences",
                table: "Preferences_FinancePreferences",
                column: "PreferenceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_GeneralPreferences_PreferenceId",
                schema: "UserPreferences",
                table: "Preferences_GeneralPreferences",
                column: "PreferenceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_NotificationPreferences_PreferenceId",
                schema: "UserPreferences",
                table: "Preferences_NotificationPreferences",
                column: "PreferenceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Preferences_FinancePreferences",
                schema: "UserPreferences");

            migrationBuilder.DropTable(
                name: "Preferences_GeneralPreferences",
                schema: "UserPreferences");

            migrationBuilder.DropTable(
                name: "Preferences_NotificationPreferences",
                schema: "UserPreferences");

            migrationBuilder.DropTable(
                name: "Preferences",
                schema: "UserPreferences");
        }
    }
}
