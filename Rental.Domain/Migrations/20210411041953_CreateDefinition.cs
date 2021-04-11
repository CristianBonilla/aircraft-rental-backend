using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rental.Domain.Migrations
{
    public partial class CreateDefinition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Aircraft",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<char>(type: "nvarchar(1)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aircraft", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Passenger",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    IdentificationDocument = table.Column<long>(type: "bigint", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Specialty = table.Column<string>(type: "varchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passenger", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rental",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    PassengerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AircraftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Location = table.Column<string>(type: "varchar(max)", nullable: false),
                    ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartureDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rental", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rental_Aircraft_AircraftId",
                        column: x => x.AircraftId,
                        principalSchema: "dbo",
                        principalTable: "Aircraft",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rental_Passenger_PassengerId",
                        column: x => x.PassengerId,
                        principalSchema: "dbo",
                        principalTable: "Passenger",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                schema: "dbo",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "dbo",
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdentificationDocument = table.Column<long>(type: "bigint", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "varchar(max)", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Permission",
                columns: new[] { "Id", "DisplayName", "Name", "Order" },
                values: new object[,]
                {
                    { new Guid("c5e3a53f-ce37-4512-91f3-a6d823dabe06"), "Roles", "CanRoles", 1 },
                    { new Guid("b8c5caa1-4a44-4783-af7e-eb29617a5a70"), "Usuarios", "CanUsers", 2 },
                    { new Guid("186df72b-0328-4539-8015-2965eb13ccec"), "Alquileres", "CanRentals", 3 },
                    { new Guid("44eb6612-536e-46d2-96ef-a752691f2296"), "Aeronaves", "CanAircrafts", 4 },
                    { new Guid("352dec26-951c-4236-afb5-b059f014e819"), "Pasajeros", "CanPassengers", 5 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Role",
                columns: new[] { "Id", "DisplayName", "Name" },
                values: new object[,]
                {
                    { new Guid("6bbe4b56-3f81-4957-a8f1-33c9112db4a2"), "Administrador", "AdminUser" },
                    { new Guid("22b20e06-f147-41d6-8333-7c921242ad27"), "Usuario Común", "CommonUser" },
                    { new Guid("aedb18fc-7b6c-488c-80bf-8bc2b36febe3"), "Pasajero", "PassengerUser" },
                    { new Guid("da9fbf03-d19b-4586-a28b-7b8deaa7a5b6"), "Piloto", "PilotUser" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "RolePermission",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("c5e3a53f-ce37-4512-91f3-a6d823dabe06"), new Guid("6bbe4b56-3f81-4957-a8f1-33c9112db4a2") },
                    { new Guid("b8c5caa1-4a44-4783-af7e-eb29617a5a70"), new Guid("6bbe4b56-3f81-4957-a8f1-33c9112db4a2") },
                    { new Guid("186df72b-0328-4539-8015-2965eb13ccec"), new Guid("6bbe4b56-3f81-4957-a8f1-33c9112db4a2") },
                    { new Guid("44eb6612-536e-46d2-96ef-a752691f2296"), new Guid("6bbe4b56-3f81-4957-a8f1-33c9112db4a2") },
                    { new Guid("352dec26-951c-4236-afb5-b059f014e819"), new Guid("6bbe4b56-3f81-4957-a8f1-33c9112db4a2") },
                    { new Guid("186df72b-0328-4539-8015-2965eb13ccec"), new Guid("22b20e06-f147-41d6-8333-7c921242ad27") },
                    { new Guid("352dec26-951c-4236-afb5-b059f014e819"), new Guid("22b20e06-f147-41d6-8333-7c921242ad27") },
                    { new Guid("44eb6612-536e-46d2-96ef-a752691f2296"), new Guid("aedb18fc-7b6c-488c-80bf-8bc2b36febe3") },
                    { new Guid("352dec26-951c-4236-afb5-b059f014e819"), new Guid("aedb18fc-7b6c-488c-80bf-8bc2b36febe3") },
                    { new Guid("186df72b-0328-4539-8015-2965eb13ccec"), new Guid("da9fbf03-d19b-4586-a28b-7b8deaa7a5b6") },
                    { new Guid("44eb6612-536e-46d2-96ef-a752691f2296"), new Guid("da9fbf03-d19b-4586-a28b-7b8deaa7a5b6") },
                    { new Guid("352dec26-951c-4236-afb5-b059f014e819"), new Guid("da9fbf03-d19b-4586-a28b-7b8deaa7a5b6") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Name_DisplayName",
                schema: "dbo",
                table: "Permission",
                columns: new[] { "Name", "DisplayName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rental_AircraftId",
                schema: "dbo",
                table: "Rental",
                column: "AircraftId");

            migrationBuilder.CreateIndex(
                name: "IX_Rental_PassengerId",
                schema: "dbo",
                table: "Rental",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name_DisplayName",
                schema: "dbo",
                table: "Role",
                columns: new[] { "Name", "DisplayName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                schema: "dbo",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_User_IdentificationDocument_Username_Email",
                schema: "dbo",
                table: "User",
                columns: new[] { "IdentificationDocument", "Username", "Email" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                schema: "dbo",
                table: "User",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rental",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RolePermission",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "User",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Aircraft",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Passenger",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Permission",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "dbo");
        }
    }
}
