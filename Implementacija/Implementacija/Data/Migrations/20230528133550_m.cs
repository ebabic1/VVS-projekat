using Microsoft.EntityFrameworkCore.Migrations;

namespace Implementacija.Data.Migrations
{
    public partial class m : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PorukaVeze");

            migrationBuilder.AddColumn<int>(
                name: "posiljalacId",
                table: "Poruke",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "primalacId",
                table: "Poruke",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "posiljalacId",
                table: "Poruke");

            migrationBuilder.DropColumn(
                name: "primalacId",
                table: "Poruke");

            migrationBuilder.CreateTable(
                name: "PorukaVeze",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    porukaId = table.Column<int>(type: "int", nullable: false),
                    posiljalacId = table.Column<int>(type: "int", nullable: false),
                    primalacId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PorukaVeze", x => x.Id);
                });
        }
    }
}
