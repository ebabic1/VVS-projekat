using Implementacija.Data;
using Implementacija.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Implementacija.Services
{
    public class DvoranaManager : IDvoranaManager
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _contextAccessor;
        public DvoranaManager(ApplicationDbContext db, IHttpContextAccessor contextAccessor) {
            _db = db;
            _contextAccessor = contextAccessor;
        }
        public async Task<IEnumerable<Dvorana>> GetAll() => await _db.Dvorane.ToListAsync();
        public async Task<IEnumerable<Dvorana>> GetUnreserved()
        {
            var unreservedDvorane = await _db.Dvorane
                .Where(dvorana => !_db.RezervacijaDvorana.Any(r => r.dvoranaId == dvorana.Id && r.rezervacija.potvrda))
                .ToListAsync();

            return unreservedDvorane;
        }

        public string GetUserId()
        {
            return _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task<IEnumerable<Dvorana>> GetReservedByCurrentPerformer()
        {
            var reservedDvorane = await _db.Dvorane
                .Where(dvorana => _db.RezervacijaDvorana.Any(r => r.dvoranaId == dvorana.Id && r.izvodjacId == GetUserId() && r.rezervacija.potvrda))
                .ToListAsync();
            return reservedDvorane;
        }
        public async Task<IEnumerable<Dvorana>> GetOwnedByCurrentRenter()
        {
            var rez = await _db.Dvorane.Where(d => d.iznajmljivacId == GetUserId()).ToListAsync();
            return rez;
        }
        public async Task<IEnumerable<RezervacijaDvorane>> GetReservations()
        {
            var reservedDvorane = await _db.RezervacijaDvorana
                .Where(dvorana => dvorana.dvorana.iznajmljivacId==GetUserId())
                .ToListAsync();
            return reservedDvorane;
        }
       
    }
}
