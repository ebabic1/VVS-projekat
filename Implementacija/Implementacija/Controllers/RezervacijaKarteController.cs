using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Implementacija.Data;
using Implementacija.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Security.Claims;
using Implementacija.Services;

namespace Implementacija.Controllers
{
    [Authorize]
    public class RezervacijaKarteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRezervacijaManager _rezervacijaManager;

        public RezervacijaKarteController(ApplicationDbContext context, IRezervacijaManager rezervacijaManager)
        {
            _context = context;
            _rezervacijaManager = rezervacijaManager;
        }

        // GET: RezervacijaKarte
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RezervacijaKarata.Include(r => r.koncert).Include(r => r.rezervacija);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RezervacijaKarte/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var rezervacijaKarte = await _context.RezervacijaKarata
                .Include(r => r.koncert)
                .Include(r => r.rezervacija)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (rezervacijaKarte == null)
            {
                return NotFound();
            }
            return View(rezervacijaKarte);
        }

        // Ovo se koristi za rezervaciju karte
        [Authorize(Roles = "ObicniKorisnik")]
        public async Task<IActionResult> Reserve(int? id)
        {
            if (id == null) return NotFound();
            var koncert = await _context.Koncerti
                .Include(r => r.izvodjac)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (koncert == null) return NotFound();
            var rezervacija = new RezervacijaKarte();
            rezervacija.koncert = koncert;
            ViewBag.Data = await _rezervacijaManager.GeneratePrices(koncert.Id);
            return View(rezervacija);
        }

        // GET: RezervacijaKarte/Create
        public IActionResult Create()
        {
            ViewData["koncertId"] = new SelectList(_context.Koncerti, "Id", "Id");
            ViewData["rezervacijaId"] = new SelectList(_context.Set<Rezervacija>(), "Id", "Id");
            return View();
        }

        // POST: RezervacijaKarte/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,rezervacijaId,obicniKorisnikId,tipMjesta,koncertId")] RezervacijaKarte rezervacijaKarte)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rezervacijaKarte);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["koncertId"] = new SelectList(_context.Koncerti, "Id", "Id", rezervacijaKarte.koncertId);
            ViewData["rezervacijaId"] = new SelectList(_context.Set<Rezervacija>(), "Id", "Id", rezervacijaKarte.rezervacijaId);
            return View(rezervacijaKarte);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReserve([Bind("Id,rezervacijaId,obicniKorisnikId,tipMjesta,koncertId")] RezervacijaKarte rezervacijaKarte)
        {
            // Pronalazi se koncert za koji se kreira rezervacija i provjerava da li ima preostalih mjesta na tom koncertu
            var koncertManager = new KoncertManager(_context);
            var koncert = _context.Koncerti.Where(x => x.Id == rezervacijaKarte.koncertId).SingleOrDefault();
            int remainingSeats = koncertManager.GetRemainingSeats(koncert); 
            if (ModelState.IsValid && remainingSeats > 0)
            {
                // Ako ima mjesta i podaci su validni rezervacija se kreira za trenutno ulogovanog korisnika
                var rezervacija = await KreirajRezervaciju(rezervacijaKarte);
                rezervacijaKarte.rezervacijaId = rezervacija.Id;
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                rezervacijaKarte.obicniKorisnikId = userId;
                _context.Add(rezervacijaKarte);
                await _context.SaveChangesAsync();
                
                return RedirectToAction("Index", "Home");
            }
            // Ako nema mjesta ili podaci nisu ispravni ispisuje se određena poruka i korisnik se vraća na početnu stranicu
            ViewData["koncertId"] = new SelectList(_context.Koncerti, "Id", "Id", rezervacijaKarte.koncertId);
            ViewData["rezervacijaId"] = new SelectList(_context.Set<Rezervacija>(), "Id", "Id", rezervacijaKarte.rezervacijaId);
            try
            {
                if(remainingSeats <= 0) TempData["ErrorMessage"] = "Nema slobodnih mjesta";
                else TempData["ErrorMessage"] = "Podaci nisu validni";
            }
            catch(Exception)
            {

            }
            return RedirectToAction("Index", "Home");
        }

        // GET: RezervacijaKarte/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacijaKarte = await _context.RezervacijaKarata.FindAsync(id);
            if (rezervacijaKarte == null)
            {
                return NotFound();
            }
            ViewData["koncertId"] = new SelectList(_context.Koncerti, "Id", "Id", rezervacijaKarte.koncertId);
            ViewData["rezervacijaId"] = new SelectList(_context.Set<Rezervacija>(), "Id", "Id", rezervacijaKarte.rezervacijaId);
            return View(rezervacijaKarte);
        }

        // POST: RezervacijaKarte/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,rezervacijaId,obicniKorisnikId,tipMjesta,koncertId")] RezervacijaKarte rezervacijaKarte)
        {
            if (id != rezervacijaKarte.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Update(rezervacijaKarte);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["koncertId"] = new SelectList(_context.Koncerti, "Id", "Id", rezervacijaKarte.koncertId);
            ViewData["rezervacijaId"] = new SelectList(_context.Set<Rezervacija>(), "Id", "Id", rezervacijaKarte.rezervacijaId);
            return View(rezervacijaKarte);
        }

        // GET: RezervacijaKarte/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacijaKarte = await _context.RezervacijaKarata
                .Include(r => r.koncert)
                .Include(r => r.rezervacija)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (rezervacijaKarte == null)
            {
                return NotFound();
            }

            return View(rezervacijaKarte);
        }

        // POST: RezervacijaKarte/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervacijaKarte = await _context.RezervacijaKarata.FindAsync(id);
            _context.RezervacijaKarata.Remove(rezervacijaKarte);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RezervacijaKarteExists(int id)
        {
            return _context.RezervacijaKarata.Any(e => e.Id == id);
        }
        private async Task<Rezervacija> KreirajRezervaciju(RezervacijaKarte rezervacijaKarte)
        {
            var rezervacija = new Rezervacija
            {
                cijena = await _rezervacijaManager.calculatePrice(rezervacijaKarte.tipMjesta, rezervacijaKarte.koncertId),
                potvrda = false
            };

            _context.Add(rezervacija);
            await _context.SaveChangesAsync();

            return rezervacija;
        }
    }
}
