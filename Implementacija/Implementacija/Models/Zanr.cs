using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Implementacija.Models
{
    public enum Zanr
    {
        [Display(Name = "HIPHOP")]
        HIPHOP,
        [Display(Name = "POP")]
        POP,
        [Display(Name = "ROCK")]
        ROCK
    }
}
