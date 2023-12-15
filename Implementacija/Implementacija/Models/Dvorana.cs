using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Implementacija.Models
{
    public class Dvorana
    {
        [Key]
        public int Id { get; set; }
        public string nazivDvorane { get; set; }
        public string adresaDvorane { get; set; }
        public int brojSjedista { get; set; }

        [ForeignKey("Iznajmljivac")]
        public string iznajmljivacId { get; set; }
        public Iznajmljivac iznajmljivac { get; set; }
        public Dvorana() { }
    }
}
