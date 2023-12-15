using Microsoft.EntityFrameworkCore.Migrations;

namespace Implementacija.Data.Migrations
{
    public partial class Migracija : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Iznajmljivaci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    imeIPrezime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lozinka = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Iznajmljivaci", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Izvodjaci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    imeIPrezime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lozinka = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Izvodjaci", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObicniKorisnici",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    imeIPrezime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lozinka = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObicniKorisnici", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PorukaVeze",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PorukaVeze", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Poruke",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sadrzaj = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poruke", x => x.Id);
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
                name: "Dvorane",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazivDvorane = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    adresaDvorane = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    brojSjedista = table.Column<int>(type: "int", nullable: false),
                    iznajmljivacId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dvorane", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dvorane_Iznajmljivaci_iznajmljivacId",
                        column: x => x.iznajmljivacId,
                        principalTable: "Iznajmljivaci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Koncerti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    naziv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    izvodjacId = table.Column<int>(type: "int", nullable: false),
                    zanr = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Koncerti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Koncerti_Izvodjaci_izvodjacId",
                        column: x => x.izvodjacId,
                        principalTable: "Izvodjaci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recenzije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rating = table.Column<double>(type: "float", nullable: false),
                    komentar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    izvodjacId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recenzije", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recenzije_Izvodjaci_izvodjacId",
                        column: x => x.izvodjacId,
                        principalTable: "Izvodjaci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RezervacijaDvorana",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rezervacijaId = table.Column<int>(type: "int", nullable: false),
                    izvodjacId = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_RezervacijaDvorana_Izvodjaci_izvodjacId",
                        column: x => x.izvodjacId,
                        principalTable: "Izvodjaci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_RezervacijaKarata_ObicniKorisnici_obicniKorisnikId",
                        column: x => x.obicniKorisnikId,
                        principalTable: "ObicniKorisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RezervacijaKarata_Rezervacija_rezervacijaId",
                        column: x => x.rezervacijaId,
                        principalTable: "Rezervacija",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dvorane_iznajmljivacId",
                table: "Dvorane",
                column: "iznajmljivacId");

            migrationBuilder.CreateIndex(
                name: "IX_Koncerti_izvodjacId",
                table: "Koncerti",
                column: "izvodjacId");

            migrationBuilder.CreateIndex(
                name: "IX_Recenzije_izvodjacId",
                table: "Recenzije",
                column: "izvodjacId");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaDvorana_dvoranaId",
                table: "RezervacijaDvorana",
                column: "dvoranaId");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaDvorana_izvodjacId",
                table: "RezervacijaDvorana",
                column: "izvodjacId");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaDvorana_rezervacijaId",
                table: "RezervacijaDvorana",
                column: "rezervacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaKarata_koncertId",
                table: "RezervacijaKarata",
                column: "koncertId");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaKarata_obicniKorisnikId",
                table: "RezervacijaKarata",
                column: "obicniKorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaKarata_rezervacijaId",
                table: "RezervacijaKarata",
                column: "rezervacijaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PorukaVeze");

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
        }
    }
}
