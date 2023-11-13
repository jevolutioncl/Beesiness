using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beesiness.Migrations
{
    /// <inheritdoc />
    public partial class jevo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblColmenas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoColmena = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblColmenas", x => x.Id);
                });

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
                name: "tblRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblTareas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTareas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblTipoFlor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTipoFlor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblAlertas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Detalle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IdColmena = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAlertas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblAlertas_tblColmenas_IdColmena",
                        column: x => x.IdColmena,
                        principalTable: "tblColmenas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblInfoSensores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Temperatura = table.Column<float>(type: "real", nullable: false),
                    IdColmena = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblInfoSensores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblInfoSensores_tblColmenas_IdColmena",
                        column: x => x.IdColmena,
                        principalTable: "tblColmenas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblNotas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IdColmena = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblNotas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblNotas_tblColmenas_IdColmena",
                        column: x => x.IdColmena,
                        principalTable: "tblColmenas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    IdEnfermedad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEnfermedadColmena", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblEnfermedadColmena_tblColmenas_IdColmena",
                        column: x => x.IdColmena,
                        principalTable: "tblColmenas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblEnfermedadColmena_tblEnfermedades_IdEnfermedad",
                        column: x => x.IdEnfermedad,
                        principalTable: "tblEnfermedades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblUsuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    IdRol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUsuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblUsuarios_tblRoles_IdRol",
                        column: x => x.IdRol,
                        principalTable: "tblRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblTareaColmena",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaRealizacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdColmena = table.Column<int>(type: "int", nullable: false),
                    IdTarea = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTareaColmena", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblTareaColmena_tblColmenas_IdColmena",
                        column: x => x.IdColmena,
                        principalTable: "tblColmenas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblTareaColmena_tblTareas_IdTarea",
                        column: x => x.IdTarea,
                        principalTable: "tblTareas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblPolinizaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Lugar = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IdTipoFlor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPolinizaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblPolinizaciones_tblTipoFlor_IdTipoFlor",
                        column: x => x.IdTipoFlor,
                        principalTable: "tblTipoFlor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblTratamientos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Detalle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    idEnfermedadColmena = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTratamientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblTratamientos_tblEnfermedadColmena_idEnfermedadColmena",
                        column: x => x.idEnfermedadColmena,
                        principalTable: "tblEnfermedadColmena",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblInspecciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblInspecciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblInspecciones_tblUsuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "tblUsuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblProducciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaRecoleccion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CantidadMiel = table.Column<float>(type: "real", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IdColmena = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdTipoFlor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblProducciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblProducciones_tblColmenas_IdColmena",
                        column: x => x.IdColmena,
                        principalTable: "tblColmenas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblProducciones_tblTipoFlor_IdTipoFlor",
                        column: x => x.IdTipoFlor,
                        principalTable: "tblTipoFlor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblProducciones_tblUsuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "tblUsuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblColmenaPolinizacion",
                columns: table => new
                {
                    IdPolinizacion = table.Column<int>(type: "int", nullable: false),
                    IdColmena = table.Column<int>(type: "int", nullable: false),
                    PolinizacionId = table.Column<int>(type: "int", nullable: false),
                    ColmenaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblColmenaPolinizacion", x => new { x.IdPolinizacion, x.IdColmena });
                    table.ForeignKey(
                        name: "FK_tblColmenaPolinizacion_tblColmenas_ColmenaId",
                        column: x => x.ColmenaId,
                        principalTable: "tblColmenas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblColmenaPolinizacion_tblPolinizaciones_PolinizacionId",
                        column: x => x.PolinizacionId,
                        principalTable: "tblPolinizaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblAlertas_IdColmena",
                table: "tblAlertas",
                column: "IdColmena");

            migrationBuilder.CreateIndex(
                name: "IX_tblColmenaPolinizacion_ColmenaId",
                table: "tblColmenaPolinizacion",
                column: "ColmenaId");

            migrationBuilder.CreateIndex(
                name: "IX_tblColmenaPolinizacion_PolinizacionId",
                table: "tblColmenaPolinizacion",
                column: "PolinizacionId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEnfermedadColmena_IdColmena",
                table: "tblEnfermedadColmena",
                column: "IdColmena");

            migrationBuilder.CreateIndex(
                name: "IX_tblEnfermedadColmena_IdEnfermedad",
                table: "tblEnfermedadColmena",
                column: "IdEnfermedad");

            migrationBuilder.CreateIndex(
                name: "IX_tblInfoSensores_IdColmena",
                table: "tblInfoSensores",
                column: "IdColmena");

            migrationBuilder.CreateIndex(
                name: "IX_tblInspecciones_IdUsuario",
                table: "tblInspecciones",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_tblNotas_IdColmena",
                table: "tblNotas",
                column: "IdColmena");

            migrationBuilder.CreateIndex(
                name: "IX_tblPolinizaciones_IdTipoFlor",
                table: "tblPolinizaciones",
                column: "IdTipoFlor");

            migrationBuilder.CreateIndex(
                name: "IX_tblProducciones_IdColmena",
                table: "tblProducciones",
                column: "IdColmena");

            migrationBuilder.CreateIndex(
                name: "IX_tblProducciones_IdTipoFlor",
                table: "tblProducciones",
                column: "IdTipoFlor");

            migrationBuilder.CreateIndex(
                name: "IX_tblProducciones_IdUsuario",
                table: "tblProducciones",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_tblTareaColmena_IdColmena",
                table: "tblTareaColmena",
                column: "IdColmena");

            migrationBuilder.CreateIndex(
                name: "IX_tblTareaColmena_IdTarea",
                table: "tblTareaColmena",
                column: "IdTarea");

            migrationBuilder.CreateIndex(
                name: "IX_tblTratamientos_idEnfermedadColmena",
                table: "tblTratamientos",
                column: "idEnfermedadColmena");

            migrationBuilder.CreateIndex(
                name: "IX_tblUsuarios_IdRol",
                table: "tblUsuarios",
                column: "IdRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblAlertas");

            migrationBuilder.DropTable(
                name: "tblColmenaPolinizacion");

            migrationBuilder.DropTable(
                name: "tblInfoSensores");

            migrationBuilder.DropTable(
                name: "tblInspecciones");

            migrationBuilder.DropTable(
                name: "tblNotas");

            migrationBuilder.DropTable(
                name: "tblProducciones");

            migrationBuilder.DropTable(
                name: "tblTareaColmena");

            migrationBuilder.DropTable(
                name: "tblTratamientos");

            migrationBuilder.DropTable(
                name: "tblPolinizaciones");

            migrationBuilder.DropTable(
                name: "tblUsuarios");

            migrationBuilder.DropTable(
                name: "tblTareas");

            migrationBuilder.DropTable(
                name: "tblEnfermedadColmena");

            migrationBuilder.DropTable(
                name: "tblTipoFlor");

            migrationBuilder.DropTable(
                name: "tblRoles");

            migrationBuilder.DropTable(
                name: "tblColmenas");

            migrationBuilder.DropTable(
                name: "tblEnfermedades");
        }
    }
}
