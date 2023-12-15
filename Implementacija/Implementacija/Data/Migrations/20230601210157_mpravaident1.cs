using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Implementacija.Data.Migrations
{
    public partial class mpravaident1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Korisnici",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnici", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rezervacija",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cijena = table.Column<double>(type: "float", nullable: false),
                    potvrda = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervacija", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Iznajmljivaci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
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
                    adresaDvorane = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    brojSjedista = table.Column<int>(type: "int", nullable: false),
                    iznajmljivacId = table.Column<int>(type: "int", nullable: false),
                    nazivDvorane = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    izvodjacId = table.Column<int>(type: "int", nullable: false),
                    naziv = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    izvodjacId = table.Column<int>(type: "int", nullable: false),
                    komentar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rating = table.Column<double>(type: "float", nullable: false)
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
                name: "Poruke",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    primalacId = table.Column<int>(type: "int", nullable: false),
                    sadrzaj = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poruke", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Poruke_ObicniKorisnici_primalacId",
                        column: x => x.primalacId,
                        principalTable: "ObicniKorisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RezervacijaDvorana",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dvoranaId = table.Column<int>(type: "int", nullable: false),
                    izvodjacId = table.Column<int>(type: "int", nullable: false),
                    rezervacijaId = table.Column<int>(type: "int", nullable: false)
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
                    koncertId = table.Column<int>(type: "int", nullable: false),
                    obicniKorisnikId = table.Column<int>(type: "int", nullable: false),
                    rezervacijaId = table.Column<int>(type: "int", nullable: false),
                    tipMjesta = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_Poruke_primalacId",
                table: "Poruke",
                column: "primalacId");

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
    }
}
