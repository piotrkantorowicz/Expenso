using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expenso.TimeManagement.Core.Persistence.EfCore.Migrations;

/// <inheritdoc />
public partial class MakeSomeFieldsNotNullable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<bool>(
            name: "IsCompleted",
            schema: "TimeManagement",
            table: "JobEntries",
            type: "boolean",
            nullable: false,
            defaultValue: false,
            oldClrType: typeof(bool),
            oldType: "boolean",
            oldNullable: true);

        migrationBuilder.AlterColumn<int>(
            name: "CurrentRetries",
            schema: "TimeManagement",
            table: "JobEntries",
            type: "integer",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "integer",
            oldNullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<bool>(
            name: "IsCompleted",
            schema: "TimeManagement",
            table: "JobEntries",
            type: "boolean",
            nullable: true,
            oldClrType: typeof(bool),
            oldType: "boolean");

        migrationBuilder.AlterColumn<int>(
            name: "CurrentRetries",
            schema: "TimeManagement",
            table: "JobEntries",
            type: "integer",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "integer");
    }
}