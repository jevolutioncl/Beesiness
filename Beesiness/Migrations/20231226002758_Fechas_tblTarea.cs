using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beesiness.Migrations
{
    /// <inheritdoc />
    public partial class Fechas_tblTarea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaRealizacion",
                table: "tblTareaColmena");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "tblTareaColmena");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRealizacion",
                table: "tblTareas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "tblTareas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaRealizacion",
                table: "tblTareas");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "tblTareas");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRealizacion",
                table: "tblTareaColmena",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "tblTareaColmena",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
