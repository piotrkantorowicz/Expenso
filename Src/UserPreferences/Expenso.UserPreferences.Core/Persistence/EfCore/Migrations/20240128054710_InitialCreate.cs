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
                name: "user_preferences");

            migrationBuilder.CreateTable(
                name: "preferences",
                schema: "user_preferences",
                columns: table => new
                {
                    preferences_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    use_dark_mode = table.Column<bool>(type: "boolean", nullable: true),
                    max_number_of_finance_plan_reviewers = table.Column<int>(type: "integer", nullable: true),
                    allow_add_finance_plan_reviewers = table.Column<bool>(type: "boolean", nullable: true),
                    max_number_of_sub_finance_plan_sub_owners = table.Column<int>(type: "integer", nullable: true),
                    allow_add_finance_plan_sub_owners = table.Column<bool>(type: "boolean", nullable: true),
                    send_finance_report_interval = table.Column<int>(type: "integer", nullable: true),
                    send_finance_report_enabled = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_preferences", x => x.preferences_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_preferences_user_id",
                schema: "user_preferences",
                table: "preferences",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "preferences",
                schema: "user_preferences");
        }
    }
}
