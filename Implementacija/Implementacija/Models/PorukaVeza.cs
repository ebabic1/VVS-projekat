using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Implementacija.Models
{
    public class PorukaVeza
    {
        
        [Key]
        public int Id { get; set; }
        
        
        
        public PorukaVeza() { }
    }
}
