using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Implementacija.Models
{
    public class Koncert
    {
        [Key]
        public int Id { get; set; }
        public string naziv { get; set; }
        public System.DateTime datum { get; set; }

        [ForeignKey("Izvodjac")]
        public string izvodjacId { get; set; }
        public Izvodjac izvodjac { get; set; }

        [EnumDataType(typeof(Zanr))]
        public Zanr zanr { get; set; }
        public Koncert() { }
    }
}
