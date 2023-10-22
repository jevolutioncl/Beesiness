using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beesiness.Migrations
{
    /// <inheritdoc />
    public partial class Colmena2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblEnfermedades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEnfermedades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblEnfermedadColmena",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaDeteccion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaRecuperacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdColmena = table.Column<int>(type: "int", nullable: false),
                    IdEnfermedad = table.Column<int>(type: "int", nullable: false),
                    ColmenaId = table.Column<int>(type: "int", nullable: false),
                    EnfermedadId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEnfermedadColmena", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblEnfermedadColmena_tblColmenas_ColmenaId",
                        column: x => x.ColmenaId,
                        principalTable: "tblColmenas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblEnfermedadColmena_tblEnfermedades_EnfermedadId",
                        column: x => x.EnfermedadId,
                        principalTable: "tblEnfermedades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblEnfermedadColmena_ColmenaId",
                table: "tblEnfermedadColmena",
                column: "ColmenaId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEnfermedadColmena_EnfermedadId",
                table: "tblEnfermedadColmena",
                column: "EnfermedadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblEnfermedadColmena");

            migrationBuilder.DropTable(
                name: "tblEnfermedades");
        }
    }
}
