using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beesiness.Migrations
{
    /// <inheritdoc />
    public partial class _2025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblTareaColmena_tblTareas_IdTarea",
                table: "tblTareaColmena");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblTareas",
                table: "tblTareas");

            migrationBuilder.DropColumn(
                name: "FechaRealizacion",
                table: "tblTareaColmena");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "tblTareaColmena");

            migrationBuilder.RenameTable(
                name: "tblTareas",
                newName: "Tarea");

            migrationBuilder.AddColumn<string>(
                name: "CorreoAviso",
                table: "Tarea",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRealizacion",
                table: "Tarea",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "Tarea",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Tarea",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tarea",
                table: "Tarea",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblTareaColmena_Tarea_IdTarea",
                table: "tblTareaColmena",
                column: "IdTarea",
                principalTable: "Tarea",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblTareaColmena_Tarea_IdTarea",
                table: "tblTareaColmena");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tarea",
                table: "Tarea");

            migrationBuilder.DropColumn(
                name: "CorreoAviso",
                table: "Tarea");

            migrationBuilder.DropColumn(
                name: "FechaRealizacion",
                table: "Tarea");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "Tarea");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tarea");

            migrationBuilder.RenameTable(
                name: "Tarea",
                newName: "tblTareas");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblTareas",
                table: "tblTareas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblTareaColmena_tblTareas_IdTarea",
                table: "tblTareaColmena",
                column: "IdTarea",
                principalTable: "tblTareas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
