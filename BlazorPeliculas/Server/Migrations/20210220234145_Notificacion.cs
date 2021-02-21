using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorPeliculas.Server.Migrations
{
    public partial class Notificacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notificaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    P256dh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Auth = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificaciones", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89086180-b978-4f90-9dbd-a7040bc93f41",
                column: "ConcurrencyStamp",
                value: "69c95cfc-8608-44b5-8bbb-9a2a352a132b");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notificaciones");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89086180-b978-4f90-9dbd-a7040bc93f41",
                column: "ConcurrencyStamp",
                value: "f208b55d-b9da-4b28-819a-2512e9efcb8d");
        }
    }
}
