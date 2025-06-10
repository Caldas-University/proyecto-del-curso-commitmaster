using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventLogistics.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AssignmentStart",
                table: "ResourceAssignments",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "AssignmentEnd",
                table: "ResourceAssignments",
                newName: "EndTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "ResourceAssignments",
                newName: "AssignmentStart");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "ResourceAssignments",
                newName: "AssignmentEnd");
        }
    }
}
