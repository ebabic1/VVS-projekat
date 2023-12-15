using Microsoft.EntityFrameworkCore.Migrations;

namespace Implementacija.Data.Migrations
{
    public partial class Migracija2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "porukaId",
                table: "PorukaVeze",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "posiljalacId",
                table: "PorukaVeze",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "primalacId",
                table: "PorukaVeze",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "porukaId",
                table: "PorukaVeze");

            migrationBuilder.DropColumn(
                name: "posiljalacId",
                table: "PorukaVeze");

            migrationBuilder.DropColumn(
                name: "primalacId",
                table: "PorukaVeze");
        }
    }
}
