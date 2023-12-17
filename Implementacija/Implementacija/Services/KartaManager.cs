using Implementacija.Data;
using Implementacija.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Implementacija.Services
{
    public class KartaManager : IKartaManager
    {
        private readonly ApplicationDbContext _context;
        public KartaManager(ApplicationDbContext context) => _context = context;
        public async Task<IEnumerable<RezervacijaKarte>> GetAll() => await _context.RezervacijaKarata.ToListAsync();
        public IEnumerable<RezervacijaKarte> GetOwned(string currentID)
        {
            return _context.RezervacijaKarata.Where(rez => rez.obicniKorisnikId == currentID);
        }
        public string GetName(int currentID)
        {
            var koncert = _context.Koncerti.Where(rez => rez.Id == currentID).SingleOrDefault();
            return koncert.naziv;
        }
        public string GetGuy(int currentID)
        {
            var koncert = _context.Koncerti.Where(rez => rez.Id == currentID).SingleOrDefault();
            return koncert.izvodjacId.ToString();
        }

    }
}
