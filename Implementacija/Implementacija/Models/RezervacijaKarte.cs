using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Implementacija.Models
{
    public class RezervacijaKarte
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Rezervacija")]
        public int rezervacijaId { get; set; }
        public Rezervacija rezervacija { get; set; }
        [ForeignKey("ObicniKorisnik")]
        public string obicniKorisnikId { get; set; }
        public ObicniKorisnik obicniKorisnik { get; set; }

        [EnumDataType(typeof(TipMjesta))]
        public TipMjesta tipMjesta { get; set; }

        [ForeignKey("Koncert")]
        public int koncertId { get; set; }
        public Koncert koncert { get; set; }
        public RezervacijaKarte() { }

    }
}
