﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rental.Domain;

namespace Rental.Domain.Migrations
{
    [DbContext(typeof(RentalContext))]
    partial class RentalContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Rental.Domain.AircraftEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Aircraft", "dbo");
                });

            modelBuilder.Entity("Rental.Domain.PassengerEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<long>("IdentificationDocument")
                        .HasColumnType("bigint");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Specialty")
                        .IsRequired()
                        .HasColumnType("varchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Passenger", "dbo");
                });

            modelBuilder.Entity("Rental.Domain.PermissionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Permission", "dbo");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "ROLES"
                        },
                        new
                        {
                            Id = 2,
                            Name = "USERS"
                        },
                        new
                        {
                            Id = 3,
                            Name = "AIRCRAFTS"
                        },
                        new
                        {
                            Id = 4,
                            Name = "PASSENGERS"
                        },
                        new
                        {
                            Id = 5,
                            Name = "RENTALS"
                        });
                });

            modelBuilder.Entity("Rental.Domain.RentalEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ArrivalDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DepartureDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdAircraft")
                        .HasColumnType("int");

                    b.Property<int>("IdPassenger")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("varchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdAircraft");

                    b.HasIndex("IdPassenger");

                    b.ToTable("Rental", "dbo");
                });

            modelBuilder.Entity("Rental.Domain.RoleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Role", "dbo");
                });

            modelBuilder.Entity("Rental.Domain.RolePermissionEntity", b =>
                {
                    b.Property<int>("IdRole")
                        .HasColumnType("int");

                    b.Property<int>("IdPermission")
                        .HasColumnType("int");

                    b.HasKey("IdRole", "IdPermission");

                    b.HasIndex("IdPermission");

                    b.ToTable("RolePermission", "dbo");
                });

            modelBuilder.Entity("Rental.Domain.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<int>("IdRole")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("IdRole");

                    b.ToTable("User", "dbo");
                });

            modelBuilder.Entity("Rental.Domain.RentalEntity", b =>
                {
                    b.HasOne("Rental.Domain.AircraftEntity", "Aircraft")
                        .WithMany()
                        .HasForeignKey("IdAircraft")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Rental.Domain.PassengerEntity", "Passenger")
                        .WithMany()
                        .HasForeignKey("IdPassenger")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Aircraft");

                    b.Navigation("Passenger");
                });

            modelBuilder.Entity("Rental.Domain.RolePermissionEntity", b =>
                {
                    b.HasOne("Rental.Domain.PermissionEntity", "Permission")
                        .WithMany()
                        .HasForeignKey("IdPermission")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Rental.Domain.RoleEntity", "Role")
                        .WithMany("Permissions")
                        .HasForeignKey("IdRole")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Rental.Domain.UserEntity", b =>
                {
                    b.HasOne("Rental.Domain.RoleEntity", "Role")
                        .WithMany()
                        .HasForeignKey("IdRole")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Rental.Domain.RoleEntity", b =>
                {
                    b.Navigation("Permissions");
                });
#pragma warning restore 612, 618
        }
    }
}
