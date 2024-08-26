using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusTrackerValueObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpirationDate",
                schema: "BudgetSharing",
                table: "BudgetPermissionRequests",
                newName: "StatusTracker_ExpirationDate");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "StatusTracker_ExpirationDate",
                schema: "BudgetSharing",
                table: "BudgetPermissionRequests",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StatusTracker_CancellationDate",
                schema: "BudgetSharing",
                table: "BudgetPermissionRequests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StatusTracker_ConfirmationDate",
                schema: "BudgetSharing",
                table: "BudgetPermissionRequests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StatusTracker_SubmissionDate",
                schema: "BudgetSharing",
                table: "BudgetPermissionRequests",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusTracker_CancellationDate",
                schema: "BudgetSharing",
                table: "BudgetPermissionRequests");

            migrationBuilder.DropColumn(
                name: "StatusTracker_ConfirmationDate",
                schema: "BudgetSharing",
                table: "BudgetPermissionRequests");

            migrationBuilder.DropColumn(
                name: "StatusTracker_SubmissionDate",
                schema: "BudgetSharing",
                table: "BudgetPermissionRequests");

            migrationBuilder.RenameColumn(
                name: "StatusTracker_ExpirationDate",
                schema: "BudgetSharing",
                table: "BudgetPermissionRequests",
                newName: "ExpirationDate");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ExpirationDate",
                schema: "BudgetSharing",
                table: "BudgetPermissionRequests",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");
        }
    }
}
