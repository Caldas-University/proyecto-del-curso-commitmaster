using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventLogistics.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RelacionesDeActividad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Activities",
                table: "Activities");

            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "Activities",
                newName: "UpdatedBy");

            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "ResourceAssignments",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AssignmentDate",
                table: "ResourceAssignments",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ResourceAssignments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ActivityId",
                table: "Activities",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Activities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Activities",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Activities",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Activities",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Activities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Activities",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Activities",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activities",
                table: "Activities",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceAssignments_ActivityId",
                table: "ResourceAssignments",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_EventId",
                table: "Activities",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Events_EventId",
                table: "Activities",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceAssignments_Activities_ActivityId",
                table: "ResourceAssignments",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Events_EventId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_ResourceAssignments_Activities_ActivityId",
                table: "ResourceAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ResourceAssignments_ActivityId",
                table: "ResourceAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activities",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_EventId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "ResourceAssignments");

            migrationBuilder.DropColumn(
                name: "AssignmentDate",
                table: "ResourceAssignments");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ResourceAssignments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Activities");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Activities",
                newName: "Fecha");

            migrationBuilder.AlterColumn<int>(
                name: "ActivityId",
                table: "Activities",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activities",
                table: "Activities",
                column: "ActivityId");
        }
    }
}
