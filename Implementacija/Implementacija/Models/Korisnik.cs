using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Implementacija.Models
{
    public abstract class Korisnik 
    {
        [Key]
         public string Id { get; set; }
         public string UserName { get; set; }
         public string Email { get; set; }
        public Korisnik() { }   
    }
}
