﻿// <auto-generated />
using System;
using Beesiness.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Beesiness.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Beesiness.Models.Alertas", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Detalle")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdColmena")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdColmena");

                    b.ToTable("tblAlertas");
                });

            modelBuilder.Entity("Beesiness.Models.Colmena", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<DateTime>("FechaIngreso")
                        .HasColumnType("datetime2");

                    b.Property<float>("Latitude")
                        .HasColumnType("real");

                    b.Property<float>("Longitude")
                        .HasColumnType("real");

                    b.Property<string>("TipoColmena")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("UbicacionMapaId")
                        .HasColumnType("int");

                    b.Property<int>("numIdentificador")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UbicacionMapaId");

                    b.HasIndex("numIdentificador")
                        .IsUnique();

                    b.ToTable("tblColmenas");
                });

            modelBuilder.Entity("Beesiness.Models.ColmenaPolinizacion", b =>
                {
                    b.Property<int>("IdPolinizacion")
                        .HasColumnType("int");

                    b.Property<int>("IdColmena")
                        .HasColumnType("int");

                    b.Property<int>("ColmenaId")
                        .HasColumnType("int");

                    b.Property<int>("PolinizacionId")
                        .HasColumnType("int");

                    b.HasKey("IdPolinizacion", "IdColmena");

                    b.HasIndex("ColmenaId");

                    b.HasIndex("PolinizacionId");

                    b.ToTable("tblColmenaPolinizacion");
                });

            modelBuilder.Entity("Beesiness.Models.Enfermedad", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("tblEnfermedades");
                });

            modelBuilder.Entity("Beesiness.Models.EnfermedadColmena", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("FechaDeteccion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaRecuperacion")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdColmena")
                        .HasColumnType("int");

                    b.Property<int>("IdEnfermedad")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdColmena");

                    b.HasIndex("IdEnfermedad");

                    b.ToTable("tblEnfermedadColmena");
                });

            modelBuilder.Entity("Beesiness.Models.EstadoArduino", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("ArduinoConectado")
                        .HasColumnType("bit");

                    b.Property<int>("ColmenaId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UltimaComunicacion")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ColmenaId");

                    b.ToTable("tblEstadoArduino");
                });

            modelBuilder.Entity("Beesiness.Models.InfoSensores", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdColmena")
                        .HasColumnType("int");

                    b.Property<float>("Temperatura")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("IdColmena");

                    b.ToTable("tblInfoSensores");
                });

            modelBuilder.Entity("Beesiness.Models.InformacionColmena", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("EstadoSalud")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("IdColmena")
                        .HasColumnType("int");

                    b.Property<int>("IdInspeccion")
                        .HasColumnType("int");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("TiempoVida")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UbicacionColmena")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("IdColmena");

                    b.HasIndex("IdInspeccion");

                    b.ToTable("tblInformacionColmenas");
                });

            modelBuilder.Entity("Beesiness.Models.Inspeccion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<string>("Observaciones")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("IdUsuario");

                    b.ToTable("tblInspecciones");
                });

            modelBuilder.Entity("Beesiness.Models.Nota", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdColmena")
                        .HasColumnType("int");

                    b.Property<string>("Observaciones")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("IdColmena");

                    b.ToTable("tblNotas");
                });

            modelBuilder.Entity("Beesiness.Models.Polinizacion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdTipoFlor")
                        .HasColumnType("int");

                    b.Property<string>("Lugar")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("IdTipoFlor");

                    b.ToTable("tblPolinizaciones");
                });

            modelBuilder.Entity("Beesiness.Models.Produccion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("CantidadMiel")
                        .HasColumnType("real");

                    b.Property<DateTime>("FechaRecoleccion")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdColmena")
                        .HasColumnType("int");

                    b.Property<int>("IdTipoFlor")
                        .HasColumnType("int");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<string>("Observaciones")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("IdColmena");

                    b.HasIndex("IdTipoFlor");

                    b.HasIndex("IdUsuario");

                    b.ToTable("tblProducciones");
                });

            modelBuilder.Entity("Beesiness.Models.Rol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("tblRoles");
                });

            modelBuilder.Entity("Beesiness.Models.Tarea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CorreoAviso")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<DateTime>("FechaRealizacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("tblTareas");
                });

            modelBuilder.Entity("Beesiness.Models.TareaColmena", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("IdColmena")
                        .HasColumnType("int");

                    b.Property<int>("IdTarea")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdColmena");

                    b.HasIndex("IdTarea");

                    b.ToTable("tblTareaColmena");
                });

            modelBuilder.Entity("Beesiness.Models.TipoFlor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("tblTipoFlor");
                });

            modelBuilder.Entity("Beesiness.Models.Tratamiento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Detalle")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("idEnfermedadColmena")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("idEnfermedadColmena");

                    b.ToTable("tblTratamientos");
                });

            modelBuilder.Entity("Beesiness.Models.UbicacionMapa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<float>("Latitude")
                        .HasColumnType("real");

                    b.Property<float>("Longitude")
                        .HasColumnType("real");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ZoomLevel")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("tblUbicacionMapas");
                });

            modelBuilder.Entity("Beesiness.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("IdRol")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdRol");

                    b.ToTable("tblUsuarios");
                });

            modelBuilder.Entity("Beesiness.Models.UsuarioTemporal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("FechaSolicitud")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Rol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("tblUsuariosTemporales");
                });

            modelBuilder.Entity("Beesiness.Models.Alertas", b =>
                {
                    b.HasOne("Beesiness.Models.Colmena", "Colmena")
                        .WithMany()
                        .HasForeignKey("IdColmena")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Colmena");
                });

            modelBuilder.Entity("Beesiness.Models.Colmena", b =>
                {
                    b.HasOne("Beesiness.Models.UbicacionMapa", "UbicacionMapa")
                        .WithMany()
                        .HasForeignKey("UbicacionMapaId");

                    b.Navigation("UbicacionMapa");
                });

            modelBuilder.Entity("Beesiness.Models.ColmenaPolinizacion", b =>
                {
                    b.HasOne("Beesiness.Models.Colmena", "Colmena")
                        .WithMany()
                        .HasForeignKey("ColmenaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Beesiness.Models.Polinizacion", "Polinizacion")
                        .WithMany()
                        .HasForeignKey("PolinizacionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Colmena");

                    b.Navigation("Polinizacion");
                });

            modelBuilder.Entity("Beesiness.Models.EnfermedadColmena", b =>
                {
                    b.HasOne("Beesiness.Models.Colmena", "Colmena")
                        .WithMany()
                        .HasForeignKey("IdColmena")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Beesiness.Models.Enfermedad", "Enfermedad")
                        .WithMany()
                        .HasForeignKey("IdEnfermedad")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Colmena");

                    b.Navigation("Enfermedad");
                });

            modelBuilder.Entity("Beesiness.Models.EstadoArduino", b =>
                {
                    b.HasOne("Beesiness.Models.Colmena", "Colmena")
                        .WithMany()
                        .HasForeignKey("ColmenaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Colmena");
                });

            modelBuilder.Entity("Beesiness.Models.InfoSensores", b =>
                {
                    b.HasOne("Beesiness.Models.Colmena", "Colmena")
                        .WithMany()
                        .HasForeignKey("IdColmena")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Colmena");
                });

            modelBuilder.Entity("Beesiness.Models.InformacionColmena", b =>
                {
                    b.HasOne("Beesiness.Models.Colmena", "Colmena")
                        .WithMany()
                        .HasForeignKey("IdColmena")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Beesiness.Models.Inspeccion", "Inspeccion")
                        .WithMany()
                        .HasForeignKey("IdInspeccion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Colmena");

                    b.Navigation("Inspeccion");
                });

            modelBuilder.Entity("Beesiness.Models.Inspeccion", b =>
                {
                    b.HasOne("Beesiness.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Beesiness.Models.Nota", b =>
                {
                    b.HasOne("Beesiness.Models.Colmena", "Colmena")
                        .WithMany()
                        .HasForeignKey("IdColmena")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Colmena");
                });

            modelBuilder.Entity("Beesiness.Models.Polinizacion", b =>
                {
                    b.HasOne("Beesiness.Models.TipoFlor", "TipoFlor")
                        .WithMany()
                        .HasForeignKey("IdTipoFlor")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TipoFlor");
                });

            modelBuilder.Entity("Beesiness.Models.Produccion", b =>
                {
                    b.HasOne("Beesiness.Models.Colmena", "Colmena")
                        .WithMany()
                        .HasForeignKey("IdColmena")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Beesiness.Models.TipoFlor", "TipoFlor")
                        .WithMany()
                        .HasForeignKey("IdTipoFlor")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Beesiness.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Colmena");

                    b.Navigation("TipoFlor");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Beesiness.Models.TareaColmena", b =>
                {
                    b.HasOne("Beesiness.Models.Colmena", "Colmena")
                        .WithMany()
                        .HasForeignKey("IdColmena")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Beesiness.Models.Tarea", "Tarea")
                        .WithMany()
                        .HasForeignKey("IdTarea")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Colmena");

                    b.Navigation("Tarea");
                });

            modelBuilder.Entity("Beesiness.Models.Tratamiento", b =>
                {
                    b.HasOne("Beesiness.Models.EnfermedadColmena", "EnfermedadColmena")
                        .WithMany()
                        .HasForeignKey("idEnfermedadColmena")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EnfermedadColmena");
                });

            modelBuilder.Entity("Beesiness.Models.Usuario", b =>
                {
                    b.HasOne("Beesiness.Models.Rol", "Rol")
                        .WithMany("Usuarios")
                        .HasForeignKey("IdRol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("Beesiness.Models.Rol", b =>
                {
                    b.Navigation("Usuarios");
                });
#pragma warning restore 612, 618
        }
    }
}
