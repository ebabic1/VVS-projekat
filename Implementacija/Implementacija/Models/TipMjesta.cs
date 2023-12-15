using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Implementacija.Models
{
    public enum TipMjesta
    {
        [Display(Name = "VIP")]
        VIP,
        [Display(Name = "PARTER")]
        PARTER,
        [Display(Name = "TRIBINA")]
        TRIBINA
    }
}
