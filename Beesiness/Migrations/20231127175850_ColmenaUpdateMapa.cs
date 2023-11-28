using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beesiness.Migrations
{
    /// <inheritdoc />
    public partial class ColmenaUpdateMapa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UbicacionMapaId",
                table: "tblColmenas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblColmenas_UbicacionMapaId",
                table: "tblColmenas",
                column: "UbicacionMapaId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblColmenas_tblUbicacionMapas_UbicacionMapaId",
                table: "tblColmenas",
                column: "UbicacionMapaId",
                principalTable: "tblUbicacionMapas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblColmenas_tblUbicacionMapas_UbicacionMapaId",
                table: "tblColmenas");

            migrationBuilder.DropIndex(
                name: "IX_tblColmenas_UbicacionMapaId",
                table: "tblColmenas");

            migrationBuilder.DropColumn(
                name: "UbicacionMapaId",
                table: "tblColmenas");
        }
    }
}
