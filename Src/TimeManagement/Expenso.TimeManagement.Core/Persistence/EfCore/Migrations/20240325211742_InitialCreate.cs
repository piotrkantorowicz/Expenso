using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

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
                name: "JobEntryTypes",
                schema: "TimeManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Interval = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobEntryTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobEntries",
                schema: "TimeManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobEntryTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    JobEntryStatusId = table.Column<Guid>(type: "uuid", nullable: false)
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
                        name: "FK_JobEntries_JobEntryTypes_JobEntryTypeId",
                        column: x => x.JobEntryTypeId,
                        principalSchema: "TimeManagement",
                        principalTable: "JobEntryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobEntryPeriods",
                schema: "TimeManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Interval = table.Column<int>(type: "integer", nullable: false),
                    RunAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastRun = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: true),
                    JobEntryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobEntryPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobEntryPeriods_JobEntries_JobEntryId",
                        column: x => x.JobEntryId,
                        principalSchema: "TimeManagement",
                        principalTable: "JobEntries",
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

            migrationBuilder.InsertData(
                schema: "TimeManagement",
                table: "JobEntryStatuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("214909c9-e0e2-4e93-a51c-cf1aae83a3ae"), "The job entry has failed.", "Failed" },
                    { new Guid("b465800e-dfeb-4596-9f13-77f15b17acee"), "The job entry has been cancelled.", "Cancelled" },
                    { new Guid("b9532f23-22df-433c-8603-bb79068eeb40"), "The job entry is being retried.", "Retrying" },
                    { new Guid("dc2678d1-3858-4740-aaa2-80cbfb4b69bc"), "The job entry has completed successfully.", "Completed" },
                    { new Guid("f156ddcf-5889-4d8e-8299-4d54971fe74e"), "The job entry is currently running.", "Running" }
                });

            migrationBuilder.InsertData(
                schema: "TimeManagement",
                table: "JobEntryTypes",
                columns: new[] { "Id", "Code", "Interval", "Name" },
                values: new object[] { new Guid("0185f3ae-1a77-460a-8ade-978108d41289"), "BS-REQ-EXP", 10, "Budget Sharing Requests Expiration" });

            migrationBuilder.CreateIndex(
                name: "IX_JobEntries_JobEntryStatusId",
                schema: "TimeManagement",
                table: "JobEntries",
                column: "JobEntryStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_JobEntries_JobEntryTypeId",
                schema: "TimeManagement",
                table: "JobEntries",
                column: "JobEntryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_JobEntryPeriods_JobEntryId",
                schema: "TimeManagement",
                table: "JobEntryPeriods",
                column: "JobEntryId");

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
                name: "JobEntryPeriods",
                schema: "TimeManagement");

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
                name: "JobEntryTypes",
                schema: "TimeManagement");
        }
    }
}
