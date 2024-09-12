using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expenso.TimeManagement.Core.Persistence.EfCore.Migrations;

/// <inheritdoc />
public partial class ExtendEventTypeColumnLength : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "EventType",
            schema: "TimeManagement",
            table: "JobEntryTriggers",
            type: "character varying(500)",
            maxLength: 500,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(150)",
            oldMaxLength: 150);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "EventType",
            schema: "TimeManagement",
            table: "JobEntryTriggers",
            type: "character varying(150)",
            maxLength: 150,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(500)",
            oldMaxLength: 500);
    }
}