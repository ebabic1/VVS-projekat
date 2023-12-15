using System.ComponentModel.DataAnnotations;

namespace Implementacija.Models
{
    public class Rezervacija
    {
        [Key]
        public int Id { get; set; }
        public bool potvrda { get; set; }
        public double cijena { get; set; }
        public Rezervacija() { }

    }
}
