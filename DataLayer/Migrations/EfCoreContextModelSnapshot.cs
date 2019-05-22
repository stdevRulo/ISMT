﻿// <auto-generated />
using System;
using DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataLayer.Migrations
{
    [DbContext(typeof(EfCoreContext))]
    partial class EfCoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BizData.Entities.Historial", b =>
                {
                    b.Property<int>("HistorialID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comentario");

                    b.Property<int>("Estado");

                    b.Property<DateTime>("Fecha");

                    b.Property<int?>("ItinerarioID");

                    b.Property<string>("UsuarioId");

                    b.HasKey("HistorialID");

                    b.HasIndex("ItinerarioID");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Historial");
                });

            modelBuilder.Entity("BizData.Entities.Institucion", b =>
                {
                    b.Property<int>("InstitucionID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre");

                    b.HasKey("InstitucionID");

                    b.ToTable("Instituciones");
                });

            modelBuilder.Entity("BizData.Entities.Itinerario", b =>
                {
                    b.Property<int>("ItinerarioID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Estado");

                    b.Property<DateTime?>("FechaFin");

                    b.Property<DateTime?>("FechaInicio");

                    b.Property<string>("UsuarioId");

                    b.HasKey("ItinerarioID");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Itinerarios");
                });

            modelBuilder.Entity("BizData.Entities.Pais", b =>
                {
                    b.Property<int>("PaisID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre");

                    b.Property<int?>("RegionID");

                    b.HasKey("PaisID");

                    b.HasIndex("RegionID");

                    b.ToTable("Paises");
                });

            modelBuilder.Entity("BizData.Entities.Pais_Visa", b =>
                {
                    b.Property<int>("Pais_VisaID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PaisID");

                    b.Property<int>("VisaID");

                    b.HasKey("Pais_VisaID");

                    b.HasIndex("PaisID");

                    b.HasIndex("VisaID");

                    b.ToTable("Paises_Visas");
                });

            modelBuilder.Entity("BizData.Entities.Region", b =>
                {
                    b.Property<int>("RegionID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre");

                    b.HasKey("RegionID");

                    b.ToTable("Regiones");
                });

            modelBuilder.Entity("BizData.Entities.Region_Visa", b =>
                {
                    b.Property<int>("Region_VisaID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("RegionID");

                    b.Property<int?>("VisaID");

                    b.HasKey("Region_VisaID");

                    b.HasIndex("RegionID");

                    b.HasIndex("VisaID");

                    b.ToTable("Regiones_Visa");
                });

            modelBuilder.Entity("BizData.Entities.Usuario", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstLastName");

                    b.Property<string>("FirstName");

                    b.Property<bool>("HasPassport");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecondLastName");

                    b.Property<string>("SecondName");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("BizData.Entities.Usuario_Visa", b =>
                {
                    b.Property<int>("Usuario_VisaID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("UsuarioId");

                    b.Property<int?>("VisaID");

                    b.HasKey("Usuario_VisaID");

                    b.HasIndex("UsuarioId");

                    b.HasIndex("VisaID");

                    b.ToTable("Usuario_Visa");
                });

            modelBuilder.Entity("BizData.Entities.Viaje", b =>
                {
                    b.Property<int>("ViajeID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Ciudad");

                    b.Property<DateTime?>("FechaFin");

                    b.Property<DateTime?>("FechaInicio");

                    b.Property<int?>("InstitucionID");

                    b.Property<int?>("ItinerarioID");

                    b.Property<string>("MotivoViaje");

                    b.Property<int?>("PaisID");

                    b.HasKey("ViajeID");

                    b.HasIndex("InstitucionID");

                    b.HasIndex("ItinerarioID");

                    b.HasIndex("PaisID");

                    b.ToTable("Viajes");
                });

            modelBuilder.Entity("BizData.Entities.Visa", b =>
                {
                    b.Property<int>("VisaID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("VisaID");

                    b.ToTable("Visas");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("BizData.Entities.Historial", b =>
                {
                    b.HasOne("BizData.Entities.Itinerario", "Itinerario")
                        .WithMany()
                        .HasForeignKey("ItinerarioID");

                    b.HasOne("BizData.Entities.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId");
                });

            modelBuilder.Entity("BizData.Entities.Itinerario", b =>
                {
                    b.HasOne("BizData.Entities.Usuario", "Usuario")
                        .WithMany("Itinerarios")
                        .HasForeignKey("UsuarioId");
                });

            modelBuilder.Entity("BizData.Entities.Pais", b =>
                {
                    b.HasOne("BizData.Entities.Region", "Region")
                        .WithMany("Paises")
                        .HasForeignKey("RegionID");
                });

            modelBuilder.Entity("BizData.Entities.Pais_Visa", b =>
                {
                    b.HasOne("BizData.Entities.Pais", "Pais")
                        .WithMany("Visas")
                        .HasForeignKey("PaisID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BizData.Entities.Visa", "Visa")
                        .WithMany("Paises")
                        .HasForeignKey("VisaID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BizData.Entities.Region_Visa", b =>
                {
                    b.HasOne("BizData.Entities.Region", "Region")
                        .WithMany("Visas")
                        .HasForeignKey("RegionID");

                    b.HasOne("BizData.Entities.Visa", "Visa")
                        .WithMany("Regiones")
                        .HasForeignKey("VisaID");
                });

            modelBuilder.Entity("BizData.Entities.Usuario_Visa", b =>
                {
                    b.HasOne("BizData.Entities.Usuario", "Usuario")
                        .WithMany("Visas")
                        .HasForeignKey("UsuarioId");

                    b.HasOne("BizData.Entities.Visa", "Visa")
                        .WithMany("Usuarios")
                        .HasForeignKey("VisaID");
                });

            modelBuilder.Entity("BizData.Entities.Viaje", b =>
                {
                    b.HasOne("BizData.Entities.Institucion", "Institucion")
                        .WithMany()
                        .HasForeignKey("InstitucionID");

                    b.HasOne("BizData.Entities.Itinerario", "Itinerario")
                        .WithMany("Viajes")
                        .HasForeignKey("ItinerarioID");

                    b.HasOne("BizData.Entities.Pais", "Pais")
                        .WithMany()
                        .HasForeignKey("PaisID");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BizData.Entities.Usuario")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BizData.Entities.Usuario")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BizData.Entities.Usuario")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BizData.Entities.Usuario")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
