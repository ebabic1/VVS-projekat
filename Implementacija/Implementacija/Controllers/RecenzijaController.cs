using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Implementacija.Data;
using Implementacija.Models;
using Microsoft.AspNetCore.Authorization;

namespace Implementacija.Controllers
{
    [Authorize]
    public class RecenzijaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecenzijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Recenzija
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Recenzije.Include(r => r.izvodjac);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Recenzija/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recenzija = await _context.Recenzije
                .Include(r => r.izvodjac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recenzija == null)
            {
                return NotFound();
            }

            return View(recenzija);
        }

        // Ostavljanje recenzija
        [Authorize(Roles = "ObicniKorisnik")]
        public async Task<IActionResult> LeaveReview(string? id)
        {
            if (id == null) return NotFound();
            var artist = await _context.Izvodjaci.FirstOrDefaultAsync(m => m.Id == id);
            if (artist == null) return NotFound();
            var listaRecenzija = await _context.Recenzije.Where(r => r.izvodjacId == id).ToListAsync();
            int brojac = 0;
            for(int i = 0; i < listaRecenzija.Count; i++)
            {
                if (listaRecenzija[i].izvodjacId == id) brojac++;
            }
            var recenzija = new Recenzija();
            recenzija.izvodjac = artist;
            recenzija.izvodjacId = artist.Id;
            recenzija.rating = 5;
            return View(recenzija);
        }
        // GET: Recenzija/Create
        public IActionResult Create()
        {
            ViewData["izvodjacId"] = new SelectList(_context.Izvodjaci, "Id", "Id");
            return View();
        }

        // POST: Recenzija/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,rating,komentar,izvodjacId")] Recenzija recenzija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recenzija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["izvodjacId"] = new SelectList(_context.Izvodjaci, "Id", "Id", recenzija.izvodjacId);
            return View(recenzija);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLeaveReview([Bind("Id,rating,komentar,izvodjacId")] Recenzija recenzija)
        {

            if (ModelState.IsValid)
            {
                _context.Add(recenzija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(recenzija);
        }

        // GET: Recenzija/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recenzija = await _context.Recenzije.FindAsync(id);
            if (recenzija == null)
            {
                return NotFound();
            }
            ViewData["izvodjacId"] = new SelectList(_context.Izvodjaci, "Id", "Id", recenzija.izvodjacId);
            return View(recenzija);
        }

        // POST: Recenzija/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,rating,komentar,izvodjacId")] Recenzija recenzija)
        {
            if (id != recenzija.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(recenzija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["izvodjacId"] = new SelectList(_context.Izvodjaci, "Id", "Id", recenzija.izvodjacId);
            return View(recenzija);
        }

        // GET: Recenzija/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recenzija = await _context.Recenzije
                .Include(r => r.izvodjac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recenzija == null)
            {
                return NotFound();
            }

            return View(recenzija);
        }

        // POST: Recenzija/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recenzija = await _context.Recenzije.FindAsync(id);
            RecenzijaExists(id);
            _context.Recenzije.Remove(recenzija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecenzijaExists(int id)
        {
            return _context.Recenzije.Any(e => e.Id == id);
        }
    }
}
