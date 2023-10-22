using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beesiness.Migrations
{
    /// <inheritdoc />
    public partial class Colmena3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblEnfermedadColmena_tblColmenas_ColmenaId",
                table: "tblEnfermedadColmena");

            migrationBuilder.DropForeignKey(
                name: "FK_tblEnfermedadColmena_tblEnfermedades_EnfermedadId",
                table: "tblEnfermedadColmena");

            migrationBuilder.DropIndex(
                name: "IX_tblEnfermedadColmena_ColmenaId",
                table: "tblEnfermedadColmena");

            migrationBuilder.DropIndex(
                name: "IX_tblEnfermedadColmena_EnfermedadId",
                table: "tblEnfermedadColmena");

            migrationBuilder.DropColumn(
                name: "ColmenaId",
                table: "tblEnfermedadColmena");

            migrationBuilder.DropColumn(
                name: "EnfermedadId",
                table: "tblEnfermedadColmena");

            migrationBuilder.CreateIndex(
                name: "IX_tblEnfermedadColmena_IdColmena",
                table: "tblEnfermedadColmena",
                column: "IdColmena");

            migrationBuilder.CreateIndex(
                name: "IX_tblEnfermedadColmena_IdEnfermedad",
                table: "tblEnfermedadColmena",
                column: "IdEnfermedad");

            migrationBuilder.AddForeignKey(
                name: "FK_tblEnfermedadColmena_tblColmenas_IdColmena",
                table: "tblEnfermedadColmena",
                column: "IdColmena",
                principalTable: "tblColmenas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblEnfermedadColmena_tblEnfermedades_IdEnfermedad",
                table: "tblEnfermedadColmena",
                column: "IdEnfermedad",
                principalTable: "tblEnfermedades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblEnfermedadColmena_tblColmenas_IdColmena",
                table: "tblEnfermedadColmena");

            migrationBuilder.DropForeignKey(
                name: "FK_tblEnfermedadColmena_tblEnfermedades_IdEnfermedad",
                table: "tblEnfermedadColmena");

            migrationBuilder.DropIndex(
                name: "IX_tblEnfermedadColmena_IdColmena",
                table: "tblEnfermedadColmena");

            migrationBuilder.DropIndex(
                name: "IX_tblEnfermedadColmena_IdEnfermedad",
                table: "tblEnfermedadColmena");

            migrationBuilder.AddColumn<int>(
                name: "ColmenaId",
                table: "tblEnfermedadColmena",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EnfermedadId",
                table: "tblEnfermedadColmena",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tblEnfermedadColmena_ColmenaId",
                table: "tblEnfermedadColmena",
                column: "ColmenaId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEnfermedadColmena_EnfermedadId",
                table: "tblEnfermedadColmena",
                column: "EnfermedadId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblEnfermedadColmena_tblColmenas_ColmenaId",
                table: "tblEnfermedadColmena",
                column: "ColmenaId",
                principalTable: "tblColmenas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblEnfermedadColmena_tblEnfermedades_EnfermedadId",
                table: "tblEnfermedadColmena",
                column: "EnfermedadId",
                principalTable: "tblEnfermedades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
