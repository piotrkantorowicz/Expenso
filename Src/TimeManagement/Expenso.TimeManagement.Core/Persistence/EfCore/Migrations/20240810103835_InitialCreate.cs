using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expenso.TimeManagement.Core.Persistence.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "TimeManagement");

            migrationBuilder.CreateTable(
                name: "JobEntryStatuses",
                schema: "TimeManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobEntryStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobInstances",
                schema: "TimeManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    RunningDelay = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobInstances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobEntries",
                schema: "TimeManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobInstanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    JobEntryStatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    CronExpression = table.Column<string>(type: "text", nullable: false),
                    RunAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CurrentRetries = table.Column<int>(type: "integer", nullable: true),
                    MaxRetries = table.Column<int>(type: "integer", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: true),
                    LastRun = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobEntries_JobEntryStatuses_JobEntryStatusId",
                        column: x => x.JobEntryStatusId,
                        principalSchema: "TimeManagement",
                        principalTable: "JobEntryStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobEntries_JobInstances_JobInstanceId",
                        column: x => x.JobInstanceId,
                        principalSchema: "TimeManagement",
                        principalTable: "JobInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobEntryTriggers",
                schema: "TimeManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    EventData = table.Column<string>(type: "text", nullable: false),
                    JobEntryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobEntryTriggers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobEntryTriggers_JobEntries_JobEntryId",
                        column: x => x.JobEntryId,
                        principalSchema: "TimeManagement",
                        principalTable: "JobEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobEntries_JobEntryStatusId",
                schema: "TimeManagement",
                table: "JobEntries",
                column: "JobEntryStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_JobEntries_JobInstanceId",
                schema: "TimeManagement",
                table: "JobEntries",
                column: "JobInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_JobEntryTriggers_JobEntryId",
                schema: "TimeManagement",
                table: "JobEntryTriggers",
                column: "JobEntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobEntryTriggers",
                schema: "TimeManagement");

            migrationBuilder.DropTable(
                name: "JobEntries",
                schema: "TimeManagement");

            migrationBuilder.DropTable(
                name: "JobEntryStatuses",
                schema: "TimeManagement");

            migrationBuilder.DropTable(
                name: "JobInstances",
                schema: "TimeManagement");
        }
    }
}
