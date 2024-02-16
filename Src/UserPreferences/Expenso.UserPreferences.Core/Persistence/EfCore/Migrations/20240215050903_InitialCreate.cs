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
                name: "FinancePreference",
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
                    table.PrimaryKey("PK_FinancePreference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancePreference_Preferences_PreferenceId",
                        column: x => x.PreferenceId,
                        principalSchema: "UserPreferences",
                        principalTable: "Preferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneralPreference",
                schema: "UserPreferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PreferenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    UseDarkMode = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralPreference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralPreference_Preferences_PreferenceId",
                        column: x => x.PreferenceId,
                        principalSchema: "UserPreferences",
                        principalTable: "Preferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationPreference",
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
                    table.PrimaryKey("PK_NotificationPreference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationPreference_Preferences_PreferenceId",
                        column: x => x.PreferenceId,
                        principalSchema: "UserPreferences",
                        principalTable: "Preferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancePreference_PreferenceId",
                schema: "UserPreferences",
                table: "FinancePreference",
                column: "PreferenceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralPreference_PreferenceId",
                schema: "UserPreferences",
                table: "GeneralPreference",
                column: "PreferenceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationPreference_PreferenceId",
                schema: "UserPreferences",
                table: "NotificationPreference",
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
                name: "FinancePreference",
                schema: "UserPreferences");

            migrationBuilder.DropTable(
                name: "GeneralPreference",
                schema: "UserPreferences");

            migrationBuilder.DropTable(
                name: "NotificationPreference",
                schema: "UserPreferences");

            migrationBuilder.DropTable(
                name: "Preferences",
                schema: "UserPreferences");
        }
    }
}
