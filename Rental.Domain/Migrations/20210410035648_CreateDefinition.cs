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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PassengerId = table.Column<int>(type: "int", nullable: false),
                    AircraftId = table.Column<int>(type: "int", nullable: false),
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
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
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
                columns: new[] { "Id", "DisplayName", "Name" },
                values: new object[,]
                {
                    { 1, "Roles", "CanRoles" },
                    { 2, "Usuarios", "CanUsers" },
                    { 3, "Alquileres", "CanRentals" },
                    { 4, "Aeronaves", "CanAircrafts" },
                    { 5, "Pasajeros", "CanPassengers" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Role",
                columns: new[] { "Id", "DisplayName", "Name" },
                values: new object[,]
                {
                    { 1, "Administrador", "AdminUser" },
                    { 2, "Usuario Común", "CommonUser" },
                    { 3, "Pasajero", "PassengerUser" },
                    { 4, "Piloto", "PilotUser" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "RolePermission",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 5, 1 },
                    { 3, 2 },
                    { 5, 2 },
                    { 4, 3 },
                    { 5, 3 },
                    { 3, 4 },
                    { 4, 4 },
                    { 5, 4 }
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
