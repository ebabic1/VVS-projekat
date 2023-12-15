using Microsoft.EntityFrameworkCore.Migrations;

namespace Implementacija.Data.Migrations
{
    public partial class mpravaident2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Korisnici",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnici", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Korisnici_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rezervacija",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    potvrda = table.Column<bool>(type: "bit", nullable: false),
                    cijena = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervacija", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Iznajmljivaci",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Iznajmljivaci", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Iznajmljivaci_Korisnici_Id",
                        column: x => x.Id,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Izvodjaci",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Izvodjaci", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Izvodjaci_Korisnici_Id",
                        column: x => x.Id,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ObicniKorisnici",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObicniKorisnici", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObicniKorisnici_Korisnici_Id",
                        column: x => x.Id,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dvorane",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazivDvorane = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    adresaDvorane = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    brojSjedista = table.Column<int>(type: "int", nullable: false),
                    iznajmljivacId = table.Column<int>(type: "int", nullable: false),
                    iznajmljivacId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dvorane", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dvorane_Iznajmljivaci_iznajmljivacId1",
                        column: x => x.iznajmljivacId1,
                        principalTable: "Iznajmljivaci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Koncerti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    naziv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    izvodjacId = table.Column<int>(type: "int", nullable: false),
                    izvodjacId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    zanr = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Koncerti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Koncerti_Izvodjaci_izvodjacId1",
                        column: x => x.izvodjacId1,
                        principalTable: "Izvodjaci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Recenzije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rating = table.Column<double>(type: "float", nullable: false),
                    komentar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    izvodjacId = table.Column<int>(type: "int", nullable: false),
                    izvodjacId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recenzije", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recenzije_Izvodjaci_izvodjacId1",
                        column: x => x.izvodjacId1,
                        principalTable: "Izvodjaci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Poruke",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sadrzaj = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    primalacId = table.Column<int>(type: "int", nullable: false),
                    primalacId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poruke", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Poruke_ObicniKorisnici_primalacId1",
                        column: x => x.primalacId1,
                        principalTable: "ObicniKorisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RezervacijaDvorana",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rezervacijaId = table.Column<int>(type: "int", nullable: false),
                    izvodjacId = table.Column<int>(type: "int", nullable: false),
                    izvodjacId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    dvoranaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RezervacijaDvorana", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RezervacijaDvorana_Dvorane_dvoranaId",
                        column: x => x.dvoranaId,
                        principalTable: "Dvorane",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RezervacijaDvorana_Izvodjaci_izvodjacId1",
                        column: x => x.izvodjacId1,
                        principalTable: "Izvodjaci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RezervacijaDvorana_Rezervacija_rezervacijaId",
                        column: x => x.rezervacijaId,
                        principalTable: "Rezervacija",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RezervacijaKarata",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rezervacijaId = table.Column<int>(type: "int", nullable: false),
                    obicniKorisnikId = table.Column<int>(type: "int", nullable: false),
                    obicniKorisnikId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    tipMjesta = table.Column<int>(type: "int", nullable: false),
                    koncertId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RezervacijaKarata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RezervacijaKarata_Koncerti_koncertId",
                        column: x => x.koncertId,
                        principalTable: "Koncerti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RezervacijaKarata_ObicniKorisnici_obicniKorisnikId1",
                        column: x => x.obicniKorisnikId1,
                        principalTable: "ObicniKorisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RezervacijaKarata_Rezervacija_rezervacijaId",
                        column: x => x.rezervacijaId,
                        principalTable: "Rezervacija",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dvorane_iznajmljivacId1",
                table: "Dvorane",
                column: "iznajmljivacId1");

            migrationBuilder.CreateIndex(
                name: "IX_Koncerti_izvodjacId1",
                table: "Koncerti",
                column: "izvodjacId1");

            migrationBuilder.CreateIndex(
                name: "IX_Poruke_primalacId1",
                table: "Poruke",
                column: "primalacId1");

            migrationBuilder.CreateIndex(
                name: "IX_Recenzije_izvodjacId1",
                table: "Recenzije",
                column: "izvodjacId1");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaDvorana_dvoranaId",
                table: "RezervacijaDvorana",
                column: "dvoranaId");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaDvorana_izvodjacId1",
                table: "RezervacijaDvorana",
                column: "izvodjacId1");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaDvorana_rezervacijaId",
                table: "RezervacijaDvorana",
                column: "rezervacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaKarata_koncertId",
                table: "RezervacijaKarata",
                column: "koncertId");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaKarata_obicniKorisnikId1",
                table: "RezervacijaKarata",
                column: "obicniKorisnikId1");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaKarata_rezervacijaId",
                table: "RezervacijaKarata",
                column: "rezervacijaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Poruke");

            migrationBuilder.DropTable(
                name: "Recenzije");

            migrationBuilder.DropTable(
                name: "RezervacijaDvorana");

            migrationBuilder.DropTable(
                name: "RezervacijaKarata");

            migrationBuilder.DropTable(
                name: "Dvorane");

            migrationBuilder.DropTable(
                name: "Koncerti");

            migrationBuilder.DropTable(
                name: "ObicniKorisnici");

            migrationBuilder.DropTable(
                name: "Rezervacija");

            migrationBuilder.DropTable(
                name: "Iznajmljivaci");

            migrationBuilder.DropTable(
                name: "Izvodjaci");

            migrationBuilder.DropTable(
                name: "Korisnici");
        }
    }
}
