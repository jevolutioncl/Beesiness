using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beesiness.Migrations
{
    /// <inheritdoc />
    public partial class EstadoArduino4324534 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblEstadoArduino",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArduinoConectado = table.Column<bool>(type: "bit", nullable: false),
                    UltimaComunicacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ColmenaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEstadoArduino", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblEstadoArduino_tblColmenas_ColmenaId",
                        column: x => x.ColmenaId,
                        principalTable: "tblColmenas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblEstadoArduino_ColmenaId",
                table: "tblEstadoArduino",
                column: "ColmenaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblEstadoArduino");
        }
    }
}
