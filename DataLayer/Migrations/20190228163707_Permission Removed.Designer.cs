﻿// <auto-generated />
using System;
using DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataLayer.Migrations
{
    [DbContext(typeof(EfCoreContext))]
    [Migration("20190228163707_Permission Removed")]
    partial class PermissionRemoved
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BizData.Entities.Ciudad", b =>
                {
                    b.Property<int>("CiudadID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre");

                    b.Property<string>("PaisID");

                    b.Property<int?>("ViajeID");

                    b.HasKey("CiudadID");

                    b.HasIndex("PaisID");

                    b.HasIndex("ViajeID");

                    b.ToTable("Ciudades");
                });

            modelBuilder.Entity("BizData.Entities.Institucion", b =>
                {
                    b.Property<int>("InstitucionID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre");

                    b.Property<int?>("ViajeID");

                    b.HasKey("InstitucionID");

                    b.HasIndex("ViajeID");

                    b.ToTable("Instituciones");
                });

            modelBuilder.Entity("BizData.Entities.Pais", b =>
                {
                    b.Property<string>("PaisID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ViajeID");

                    b.HasKey("PaisID");

                    b.HasIndex("ViajeID");

                    b.ToTable("Pais");
                });

            modelBuilder.Entity("BizData.Entities.Pais_Visa", b =>
                {
                    b.Property<int>("Pais_VisaID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PaisID");

                    b.Property<string>("PaisID1");

                    b.Property<int>("ViajeID");

                    b.Property<int?>("VisaID");

                    b.HasKey("Pais_VisaID");

                    b.HasIndex("PaisID1");

                    b.HasIndex("ViajeID");

                    b.HasIndex("VisaID");

                    b.ToTable("Pais_Visa");
                });

            modelBuilder.Entity("BizData.Entities.Pasaporte", b =>
                {
                    b.Property<int>("PasaporteID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Actualizaciones");

                    b.Property<DateTime>("FechaCreacion");

                    b.Property<DateTime>("FechaVencimiento");

                    b.Property<int>("Tipo");

                    b.Property<long>("UsuarioCI");

                    b.Property<int>("UsuarioID");

                    b.Property<long?>("UsuarioID1");

                    b.HasKey("PasaporteID");

                    b.HasIndex("UsuarioID1");

                    b.ToTable("Pasaporte");
                });

            modelBuilder.Entity("BizData.Entities.Pasaporte_Visa", b =>
                {
                    b.Property<int>("Pasaporte_VisaID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PasaporteID");

                    b.Property<int>("VisaID");

                    b.HasKey("Pasaporte_VisaID");

                    b.HasIndex("PasaporteID");

                    b.HasIndex("VisaID");

                    b.ToTable("Pasaporte_Visa");
                });

            modelBuilder.Entity("BizData.Entities.Usuario", b =>
                {
                    b.Property<long>("UsuarioID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("FirstLastName");

                    b.Property<string>("FirstName");

                    b.Property<string>("Password");

                    b.Property<int>("Permission");

                    b.Property<string>("SecondLastName");

                    b.Property<string>("SecondName");

                    b.HasKey("UsuarioID");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("BizData.Entities.Viaje", b =>
                {
                    b.Property<int>("ViajeID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Costo");

                    b.Property<DateTime>("FechaFin");

                    b.Property<DateTime>("FechaInicio");

                    b.Property<int>("MotivoViaje");

                    b.Property<int>("UsuarioID");

                    b.Property<long?>("UsuarioID1");

                    b.HasKey("ViajeID");

                    b.HasIndex("UsuarioID1");

                    b.ToTable("Viaje");
                });

            modelBuilder.Entity("BizData.Entities.Visa", b =>
                {
                    b.Property<int>("VisaID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("VisaID");

                    b.ToTable("Visa");
                });

            modelBuilder.Entity("BizData.Entities.Ciudad", b =>
                {
                    b.HasOne("BizData.Entities.Pais", "Pais")
                        .WithMany()
                        .HasForeignKey("PaisID");

                    b.HasOne("BizData.Entities.Viaje")
                        .WithMany("Ciudades")
                        .HasForeignKey("ViajeID");
                });

            modelBuilder.Entity("BizData.Entities.Institucion", b =>
                {
                    b.HasOne("BizData.Entities.Viaje")
                        .WithMany("Instituciones")
                        .HasForeignKey("ViajeID");
                });

            modelBuilder.Entity("BizData.Entities.Pais", b =>
                {
                    b.HasOne("BizData.Entities.Viaje")
                        .WithMany("Pais")
                        .HasForeignKey("ViajeID");
                });

            modelBuilder.Entity("BizData.Entities.Pais_Visa", b =>
                {
                    b.HasOne("BizData.Entities.Pais", "Pais")
                        .WithMany("Visas")
                        .HasForeignKey("PaisID1");

                    b.HasOne("BizData.Entities.Viaje", "Viaje")
                        .WithMany()
                        .HasForeignKey("ViajeID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BizData.Entities.Visa")
                        .WithMany("Paises")
                        .HasForeignKey("VisaID");
                });

            modelBuilder.Entity("BizData.Entities.Pasaporte", b =>
                {
                    b.HasOne("BizData.Entities.Usuario", "Usuario")
                        .WithMany("Pasaportes")
                        .HasForeignKey("UsuarioID1");
                });

            modelBuilder.Entity("BizData.Entities.Pasaporte_Visa", b =>
                {
                    b.HasOne("BizData.Entities.Pasaporte", "Pasaporte")
                        .WithMany("Visas")
                        .HasForeignKey("PasaporteID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BizData.Entities.Visa", "Visa")
                        .WithMany("Pasaportes")
                        .HasForeignKey("VisaID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BizData.Entities.Viaje", b =>
                {
                    b.HasOne("BizData.Entities.Usuario", "Usuario")
                        .WithMany("Viajes")
                        .HasForeignKey("UsuarioID1");
                });
#pragma warning restore 612, 618
        }
    }
}