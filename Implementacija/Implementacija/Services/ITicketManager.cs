using Implementacija.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Implementacija.Services
{
    public interface ITicketManager 
    {
        public Task<IEnumerable<RezervacijaKarte>> GetAll();
        public IEnumerable<RezervacijaKarte> GetOwned(string currentID);
        public string GetName(int currentID);
        public string GetGuy(int currentID);
    }
}
