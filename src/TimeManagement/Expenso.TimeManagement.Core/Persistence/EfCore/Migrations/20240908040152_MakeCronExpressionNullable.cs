using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expenso.TimeManagement.Core.Persistence.EfCore.Migrations;

/// <inheritdoc />
public partial class MakeCronExpressionNullable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "CronExpression",
            schema: "TimeManagement",
            table: "JobEntries",
            type: "text",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "text");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "CronExpression",
            schema: "TimeManagement",
            table: "JobEntries",
            type: "text",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "text",
            oldNullable: true);
    }
}