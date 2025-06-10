using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventLogistics.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MigracionActual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Resources",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "ReassignmentRules",
                type: "INTEGER",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ReassignmentRules",
                type: "INTEGER",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<double>(
                name: "SimilarityThreshold",
                table: "ReassignmentRules",
                type: "REAL",
                nullable: false,
                defaultValue: 0.5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "SimilarityThreshold",
                table: "ReassignmentRules");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "ReassignmentRules",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ReassignmentRules",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: true);
        }
    }
}
