using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Migrations;

/// <inheritdoc />
public partial class AddIndexForParticipantIdInPermissions : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_Permissions_ParticipantId",
            schema: "BudgetSharing",
            table: "Permissions",
            column: "ParticipantId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Permissions_ParticipantId",
            schema: "BudgetSharing",
            table: "Permissions");
    }
}