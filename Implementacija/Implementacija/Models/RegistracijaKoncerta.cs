using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Implementacija.Models
{
    public class RegistracijaKoncerta
    {

        [Key]
        public int Id { get; set; }
        public string naziv { get; set; }
        public DateTime datum { get; set; }

        [ForeignKey("Izvodjac")]
        public string izvodjacId { get; set; }
        public Izvodjac izvodjac { get; set; }

        [EnumDataType(typeof(Zanr))]
        public Zanr zanr { get; set; }

        public RegistracijaKoncerta() { }
    }
}
