using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beesiness.Migrations
{
    /// <inheritdoc />
    public partial class elias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblInformacionColmenas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UbicacionColmena = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TiempoVida = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EstadoSalud = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdInspeccion = table.Column<int>(type: "int", nullable: false),
                    IdColmena = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblInformacionColmenas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblInformacionColmenas_tblColmenas_IdColmena",
                        column: x => x.IdColmena,
                        principalTable: "tblColmenas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblInformacionColmenas_tblInspecciones_IdInspeccion",
                        column: x => x.IdInspeccion,
                        principalTable: "tblInspecciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblInformacionColmenas_IdColmena",
                table: "tblInformacionColmenas",
                column: "IdColmena");

            migrationBuilder.CreateIndex(
                name: "IX_tblInformacionColmenas_IdInspeccion",
                table: "tblInformacionColmenas",
                column: "IdInspeccion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblInformacionColmenas");
        }
    }
}
