using Microsoft.EntityFrameworkCore.Migrations;

namespace Implementacija.Data.Migrations
{
    public partial class m1234 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dvorane_Iznajmljivaci_iznajmljivacId1",
                table: "Dvorane");

            migrationBuilder.DropForeignKey(
                name: "FK_Poruke_ObicniKorisnici_primalacId1",
                table: "Poruke");

            migrationBuilder.DropForeignKey(
                name: "FK_Recenzije_Izvodjaci_izvodjacId1",
                table: "Recenzije");

            migrationBuilder.DropForeignKey(
                name: "FK_RezervacijaDvorana_Izvodjaci_izvodjacId1",
                table: "RezervacijaDvorana");

            migrationBuilder.DropForeignKey(
                name: "FK_RezervacijaKarata_ObicniKorisnici_obicniKorisnikId1",
                table: "RezervacijaKarata");

            migrationBuilder.DropIndex(
                name: "IX_RezervacijaKarata_obicniKorisnikId1",
                table: "RezervacijaKarata");

            migrationBuilder.DropIndex(
                name: "IX_RezervacijaDvorana_izvodjacId1",
                table: "RezervacijaDvorana");

            migrationBuilder.DropIndex(
                name: "IX_Recenzije_izvodjacId1",
                table: "Recenzije");

            migrationBuilder.DropIndex(
                name: "IX_Poruke_primalacId1",
                table: "Poruke");

            migrationBuilder.DropIndex(
                name: "IX_Dvorane_iznajmljivacId1",
                table: "Dvorane");

            migrationBuilder.DropColumn(
                name: "obicniKorisnikId1",
                table: "RezervacijaKarata");

            migrationBuilder.DropColumn(
                name: "izvodjacId1",
                table: "RezervacijaDvorana");

            migrationBuilder.DropColumn(
                name: "izvodjacId1",
                table: "Recenzije");

            migrationBuilder.DropColumn(
                name: "primalacId1",
                table: "Poruke");

            migrationBuilder.DropColumn(
                name: "iznajmljivacId1",
                table: "Dvorane");

            migrationBuilder.AlterColumn<string>(
                name: "obicniKorisnikId",
                table: "RezervacijaKarata",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "izvodjacId",
                table: "RezervacijaDvorana",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "izvodjacId",
                table: "Recenzije",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "primalacId",
                table: "Poruke",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "iznajmljivacId",
                table: "Dvorane",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaKarata_obicniKorisnikId",
                table: "RezervacijaKarata",
                column: "obicniKorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaDvorana_izvodjacId",
                table: "RezervacijaDvorana",
                column: "izvodjacId");

            migrationBuilder.CreateIndex(
                name: "IX_Recenzije_izvodjacId",
                table: "Recenzije",
                column: "izvodjacId");

            migrationBuilder.CreateIndex(
                name: "IX_Poruke_primalacId",
                table: "Poruke",
                column: "primalacId");

            migrationBuilder.CreateIndex(
                name: "IX_Dvorane_iznajmljivacId",
                table: "Dvorane",
                column: "iznajmljivacId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dvorane_Iznajmljivaci_iznajmljivacId",
                table: "Dvorane",
                column: "iznajmljivacId",
                principalTable: "Iznajmljivaci",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Poruke_ObicniKorisnici_primalacId",
                table: "Poruke",
                column: "primalacId",
                principalTable: "ObicniKorisnici",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recenzije_Izvodjaci_izvodjacId",
                table: "Recenzije",
                column: "izvodjacId",
                principalTable: "Izvodjaci",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RezervacijaDvorana_Izvodjaci_izvodjacId",
                table: "RezervacijaDvorana",
                column: "izvodjacId",
                principalTable: "Izvodjaci",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RezervacijaKarata_ObicniKorisnici_obicniKorisnikId",
                table: "RezervacijaKarata",
                column: "obicniKorisnikId",
                principalTable: "ObicniKorisnici",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dvorane_Iznajmljivaci_iznajmljivacId",
                table: "Dvorane");

            migrationBuilder.DropForeignKey(
                name: "FK_Poruke_ObicniKorisnici_primalacId",
                table: "Poruke");

            migrationBuilder.DropForeignKey(
                name: "FK_Recenzije_Izvodjaci_izvodjacId",
                table: "Recenzije");

            migrationBuilder.DropForeignKey(
                name: "FK_RezervacijaDvorana_Izvodjaci_izvodjacId",
                table: "RezervacijaDvorana");

            migrationBuilder.DropForeignKey(
                name: "FK_RezervacijaKarata_ObicniKorisnici_obicniKorisnikId",
                table: "RezervacijaKarata");

            migrationBuilder.DropIndex(
                name: "IX_RezervacijaKarata_obicniKorisnikId",
                table: "RezervacijaKarata");

            migrationBuilder.DropIndex(
                name: "IX_RezervacijaDvorana_izvodjacId",
                table: "RezervacijaDvorana");

            migrationBuilder.DropIndex(
                name: "IX_Recenzije_izvodjacId",
                table: "Recenzije");

            migrationBuilder.DropIndex(
                name: "IX_Poruke_primalacId",
                table: "Poruke");

            migrationBuilder.DropIndex(
                name: "IX_Dvorane_iznajmljivacId",
                table: "Dvorane");

            migrationBuilder.AlterColumn<int>(
                name: "obicniKorisnikId",
                table: "RezervacijaKarata",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "obicniKorisnikId1",
                table: "RezervacijaKarata",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "izvodjacId",
                table: "RezervacijaDvorana",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "izvodjacId1",
                table: "RezervacijaDvorana",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "izvodjacId",
                table: "Recenzije",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "izvodjacId1",
                table: "Recenzije",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "primalacId",
                table: "Poruke",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "primalacId1",
                table: "Poruke",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "iznajmljivacId",
                table: "Dvorane",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "iznajmljivacId1",
                table: "Dvorane",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaKarata_obicniKorisnikId1",
                table: "RezervacijaKarata",
                column: "obicniKorisnikId1");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaDvorana_izvodjacId1",
                table: "RezervacijaDvorana",
                column: "izvodjacId1");

            migrationBuilder.CreateIndex(
                name: "IX_Recenzije_izvodjacId1",
                table: "Recenzije",
                column: "izvodjacId1");

            migrationBuilder.CreateIndex(
                name: "IX_Poruke_primalacId1",
                table: "Poruke",
                column: "primalacId1");

            migrationBuilder.CreateIndex(
                name: "IX_Dvorane_iznajmljivacId1",
                table: "Dvorane",
                column: "iznajmljivacId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Dvorane_Iznajmljivaci_iznajmljivacId1",
                table: "Dvorane",
                column: "iznajmljivacId1",
                principalTable: "Iznajmljivaci",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Poruke_ObicniKorisnici_primalacId1",
                table: "Poruke",
                column: "primalacId1",
                principalTable: "ObicniKorisnici",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recenzije_Izvodjaci_izvodjacId1",
                table: "Recenzije",
                column: "izvodjacId1",
                principalTable: "Izvodjaci",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RezervacijaDvorana_Izvodjaci_izvodjacId1",
                table: "RezervacijaDvorana",
                column: "izvodjacId1",
                principalTable: "Izvodjaci",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RezervacijaKarata_ObicniKorisnici_obicniKorisnikId1",
                table: "RezervacijaKarata",
                column: "obicniKorisnikId1",
                principalTable: "ObicniKorisnici",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
