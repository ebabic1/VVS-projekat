using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Implementacija.Models
{
    public class RezervacijaDvorane
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Rezervacija")]
        public int rezervacijaId { get; set; }
        public Rezervacija rezervacija { get; set; }

        [ForeignKey("Izvodjac")]
        public string izvodjacId { get; set; }
        public Izvodjac izvodjac { get; set; }

        [ForeignKey("Dvorana")]
        public int dvoranaId { get; set; }
        public Dvorana dvorana { get; set; }
        public RezervacijaDvorane() { }
    }
}
