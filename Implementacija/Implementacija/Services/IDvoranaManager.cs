using Implementacija.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Implementacija.Services
{
    public interface IDvoranaManager
    {
        public Task<IEnumerable<Dvorana>> GetAll();
        public Task<IEnumerable<Dvorana>> GetUnreserved();
        public Task<IEnumerable<Dvorana>> GetReservedByCurrentPerformer();
        public Task<IEnumerable<Dvorana>> GetOwnedByCurrentRenter();
        public Task<IEnumerable<RezervacijaDvorane>> GetReservations();
    }
}
