using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Implementacija.Models
{
    public class Recenzija
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(1.0, 10.0, ErrorMessage = "Rating mora biti između 1 i 10!")]
        public double rating { get; set; }
        public string komentar { get; set; }
        [ForeignKey("Izvodjac")]
        public string izvodjacId { get; set; }
        public Izvodjac izvodjac { get; set; }
        public Recenzija() { }
    }
}
