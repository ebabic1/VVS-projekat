using Microsoft.EntityFrameworkCore.Migrations;

namespace Implementacija.Data.Migrations
{
    public partial class m123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Koncerti_Izvodjaci_izvodjacId1",
                table: "Koncerti");

            migrationBuilder.DropIndex(
                name: "IX_Koncerti_izvodjacId1",
                table: "Koncerti");

            migrationBuilder.DropColumn(
                name: "izvodjacId1",
                table: "Koncerti");

            migrationBuilder.AlterColumn<string>(
                name: "izvodjacId",
                table: "Koncerti",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Koncerti_izvodjacId",
                table: "Koncerti",
                column: "izvodjacId");

            migrationBuilder.AddForeignKey(
                name: "FK_Koncerti_Izvodjaci_izvodjacId",
                table: "Koncerti",
                column: "izvodjacId",
                principalTable: "Izvodjaci",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Koncerti_Izvodjaci_izvodjacId",
                table: "Koncerti");

            migrationBuilder.DropIndex(
                name: "IX_Koncerti_izvodjacId",
                table: "Koncerti");

            migrationBuilder.AlterColumn<int>(
                name: "izvodjacId",
                table: "Koncerti",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "izvodjacId1",
                table: "Koncerti",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Koncerti_izvodjacId1",
                table: "Koncerti",
                column: "izvodjacId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Koncerti_Izvodjaci_izvodjacId1",
                table: "Koncerti",
                column: "izvodjacId1",
                principalTable: "Izvodjaci",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
