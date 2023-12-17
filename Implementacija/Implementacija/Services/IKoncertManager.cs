using Implementacija.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Implementacija.Services
{
    public interface IKoncertManager
    {
        public  Task<IEnumerable<Koncert>> GetAll();
        public IEnumerable<Koncert> GetRecommended();
        public int GetRemainingSeats(Koncert koncert);
        public Task<IEnumerable<Koncert>> SortAktuelni(string aktuelniSortOrder, string searchString);
    }
}
