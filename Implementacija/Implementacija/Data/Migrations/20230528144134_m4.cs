using Microsoft.EntityFrameworkCore.Migrations;

namespace Implementacija.Data.Migrations
{
    public partial class m4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "posiljalacId",
                table: "Poruke");

            migrationBuilder.CreateIndex(
                name: "IX_Poruke_primalacId",
                table: "Poruke",
                column: "primalacId");

            migrationBuilder.AddForeignKey(
                name: "FK_Poruke_ObicniKorisnici_primalacId",
                table: "Poruke",
                column: "primalacId",
                principalTable: "ObicniKorisnici",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Poruke_ObicniKorisnici_primalacId",
                table: "Poruke");

            migrationBuilder.DropIndex(
                name: "IX_Poruke_primalacId",
                table: "Poruke");

            migrationBuilder.AddColumn<int>(
                name: "posiljalacId",
                table: "Poruke",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
