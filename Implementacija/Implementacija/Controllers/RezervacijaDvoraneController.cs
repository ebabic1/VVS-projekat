using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Implementacija.Data;
using Implementacija.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;
using Implementacija.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Implementacija.Controllers
{
    [Authorize]
    public class RezervacijaDvoraneController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRezervacijaManager _rezervacijaManager;

        public RezervacijaDvoraneController(ApplicationDbContext context, IRezervacijaManager rezervacijaManager)
        {
            
            _context = context;
            _rezervacijaManager = rezervacijaManager;
        }

        // GET: RezervacijaDvorane
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RezervacijaDvorana.Include(r => r.dvorana).Include(r => r.izvodjac).Include(r => r.rezervacija);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RezervacijaDvorane/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacijaDvorane = await _context.RezervacijaDvorana
                .Include(r => r.dvorana)
                .Include(r => r.izvodjac)
                .Include(r => r.rezervacija)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervacijaDvorane == null)
            {
                return NotFound();
            }

            return View(rezervacijaDvorane);
        }

        // Kreiranje Rezervacija
        [Authorize(Roles = "Izvodjac")]
        public async Task<IActionResult> Reserve(int? id)
        {
            if (id == null) return NotFound();
            var dvorana = await _context.Dvorane
                .Include(r => r.iznajmljivac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dvorana == null) return NotFound();
            var rezervacija1 = new RezervacijaDvorane();
            rezervacija1.dvorana = dvorana;
            rezervacija1.dvoranaId=dvorana.Id;
            return View(rezervacija1);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReserve(RezervacijaDvorane rezervacijaDvorane)
        {
            var dvorana = _context.Dvorane.Where(x => x.Id == rezervacijaDvorane.dvoranaId).FirstOrDefault();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var artist = _context.Izvodjaci.Where(x => x.Id == userId).FirstOrDefault();
            if (ModelState.IsValid && _rezervacijaManager.ValidateReservation(dvorana))
            {
                var rezervacija = new Rezervacija();
                rezervacija.cijena = 0;
                rezervacija.potvrda = true;
                _context.Add(rezervacija);
                rezervacijaDvorane.rezervacija = rezervacija;
                rezervacijaDvorane.rezervacijaId = rezervacija.Id;
                rezervacijaDvorane.izvodjacId = userId;
                rezervacijaDvorane.izvodjac = artist;
                rezervacijaDvorane.dvorana = dvorana;
                rezervacijaDvorane.dvoranaId = dvorana.Id;
                _context.Add(rezervacijaDvorane);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index","Home");
            }
            ViewData["koncertId"] = new SelectList(_context.Koncerti, "Id", "Id", rezervacijaDvorane.dvoranaId);
            ViewData["rezervacijaId"] = new SelectList(_context.Set<Rezervacija>(), "Id", "Id", rezervacijaDvorane.rezervacijaId);
            TempData["ErrorMessage"] = "Vec ste rezervisali neku dvoranu.";
            return RedirectToAction("Index", "Home");
        }

        // GET: RezervacijaDvorane/Create
        public IActionResult Create()
        {
            ViewData["dvoranaId"] = new SelectList(_context.Dvorane, "Id", "Id");
            ViewData["izvodjacId"] = new SelectList(_context.Izvodjaci, "Id", "Id");
            ViewData["rezervacijaId"] = new SelectList(_context.Set<Rezervacija>(), "Id", "Id");
            return View();
        }

        // POST: RezervacijaDvorane/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,rezervacijaId,izvodjacId,dvoranaId")] RezervacijaDvorane rezervacijaDvorane)
        {
            var dvorana = _context.Dvorane.Where(o => o.Id == rezervacijaDvorane.dvoranaId).FirstOrDefault();
            if (ModelState.IsValid && _rezervacijaManager.ValidateReservation(dvorana))
            {
                _context.Add(rezervacijaDvorane);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["dvoranaId"] = new SelectList(_context.Dvorane, "Id", "Id", rezervacijaDvorane.dvoranaId);
            ViewData["izvodjacId"] = new SelectList(_context.Izvodjaci, "Id", "Id", rezervacijaDvorane.izvodjacId);
            ViewData["rezervacijaId"] = new SelectList(_context.Set<Rezervacija>(), "Id", "Id", rezervacijaDvorane.rezervacijaId);
            return View(rezervacijaDvorane);
        }

        // GET: RezervacijaDvorane/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacijaDvorane = await _context.RezervacijaDvorana.FindAsync(id);
            if (rezervacijaDvorane == null)
            {
                return NotFound();
            }
            ViewData["dvoranaId"] = new SelectList(_context.Dvorane, "Id", "Id", rezervacijaDvorane.dvoranaId);
            ViewData["izvodjacId"] = new SelectList(_context.Izvodjaci, "Id", "Id", rezervacijaDvorane.izvodjacId);
            ViewData["rezervacijaId"] = new SelectList(_context.Set<Rezervacija>(), "Id", "Id", rezervacijaDvorane.rezervacijaId);
            return View(rezervacijaDvorane);
        }

        // POST: RezervacijaDvorane/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,rezervacijaId,izvodjacId,dvoranaId")] RezervacijaDvorane rezervacijaDvorane)
        {
            if (id != rezervacijaDvorane.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervacijaDvorane);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervacijaDvoraneExists(rezervacijaDvorane.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["dvoranaId"] = new SelectList(_context.Dvorane, "Id", "Id", rezervacijaDvorane.dvoranaId);
            ViewData["izvodjacId"] = new SelectList(_context.Izvodjaci, "Id", "Id", rezervacijaDvorane.izvodjacId);
            ViewData["rezervacijaId"] = new SelectList(_context.Set<Rezervacija>(), "Id", "Id", rezervacijaDvorane.rezervacijaId);
            return View(rezervacijaDvorane);
        }

        // GET: RezervacijaDvorane/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacijaDvorane = await _context.RezervacijaDvorana
                .Include(r => r.dvorana)
                .Include(r => r.izvodjac)
                .Include(r => r.rezervacija)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervacijaDvorane == null)
            {
                return NotFound();
            }

            return View(rezervacijaDvorane);
        }

        // POST: RezervacijaDvorane/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervacijaDvorane = await _context.RezervacijaDvorana.Include(r => r.rezervacija).FirstOrDefaultAsync(m => m.Id == id);
            _context.Rezervacija.Remove(rezervacijaDvorane.rezervacija);
            var koncert = _context.Koncerti.FirstOrDefaultAsync(k => k.izvodjacId == rezervacijaDvorane.izvodjacId);
            if (koncert.Result != null)
            {
                _context.Koncerti.Remove(koncert.Result);
            }
            _context.RezervacijaDvorana.Remove(rezervacijaDvorane);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RezervacijaDvoraneExists(int id)
        {
            return _context.RezervacijaDvorana.Any(e => e.Id == id);
        }
    }
}
