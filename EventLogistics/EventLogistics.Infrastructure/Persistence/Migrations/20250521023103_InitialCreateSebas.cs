using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventLogistics.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateSebas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Resources",
                newName: "TipoEquipo");

            migrationBuilder.RenameColumn(
                name: "Capacity",
                table: "Resources",
                newName: "Cantidad");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaFin",
                table: "Resources",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicio",
                table: "Resources",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Organizators",
                columns: table => new
                {
                    OrganizatorId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizators", x => x.OrganizatorId);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    ActivityId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OrganizatorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.ActivityId);
                    table.ForeignKey(
                        name: "FK_Activities_Organizators_OrganizatorId",
                        column: x => x.OrganizatorId,
                        principalTable: "Organizators",
                        principalColumn: "OrganizatorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_OrganizatorId",
                table: "Activities",
                column: "OrganizatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Organizators");

            migrationBuilder.DropColumn(
                name: "FechaFin",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "FechaInicio",
                table: "Resources");

            migrationBuilder.RenameColumn(
                name: "TipoEquipo",
                table: "Resources",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Cantidad",
                table: "Resources",
                newName: "Capacity");
        }
    }
}
