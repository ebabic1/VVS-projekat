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
using Implementacija.Services;
using System.Security.Claims;

namespace Implementacija.Controllers
{
    [Authorize]
    public class KoncertController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KoncertController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Koncert
        public async Task<IActionResult> Index()
        {
            return View(await _context.Koncerti.ToListAsync());
        }

        // GET: Koncert/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var koncert = await _context.Koncerti
                .FirstOrDefaultAsync(m => m.Id == id);
            if (koncert == null)
            {
                return NotFound();
            }

            return View(koncert);
        }

        //[Authorize(Roles = "Izvodjac")]
        // GET: Koncert/Create
        public IActionResult Create()
        {
            ViewData["izvodjacId"] = new SelectList(_context.Izvodjaci, "Id", "Id");
            return View();
        }

        // POST: Koncert/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,naziv,datum,izvodjacId,zanr")] Koncert koncert)
        {
            if (ModelState.IsValid)
           {
                _context.Add(koncert);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["izvodjacId"] = new SelectList(_context.Izvodjaci, "Id", "Id", koncert.izvodjacId);
            return View(koncert);
        }
        // Ovo se koristi za rezervaciju koncerta
        [Authorize(Roles = "Iznajmljivac")]
        public async Task<IActionResult> CreateKoncert(string id)
        {
            if (id == null) return NotFound();
            var artist = await _context.Izvodjaci.FirstOrDefaultAsync(m => m.Id == id);
            if (artist == null) return NotFound();
            var koncert = new Koncert();
            koncert.izvodjac = artist;
            koncert.izvodjacId = artist.Id;
            return View(koncert);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateKoncert([Bind("Id,naziv,izvodjacId,zanr,datum")] Koncert koncert)
        {

            _context.Add(koncert);
            await _context.SaveChangesAsync();

            ViewData["izvodjacId"] = new SelectList(_context.Izvodjaci, "Id", "Id", koncert.izvodjacId);
            return RedirectToAction("Index", "Home");
        }
        // GET: Koncert/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var koncert = await _context.Koncerti.FindAsync(id);
            if (koncert == null)
            {
                return NotFound();
            }
            return View(koncert);
        }

        // POST: Koncert/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,naziv,izvodjacId,zanr")] Koncert koncert)
        {
            if (id != koncert.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(koncert);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KoncertExists(koncert.Id))
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
            return View(koncert);
        }

        // GET: Koncert/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var koncert = await _context.Koncerti
                .FirstOrDefaultAsync(m => m.Id == id);
            if (koncert == null)
            {
                return NotFound();
            }

            return View(koncert);
        }

        // POST: Koncert/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var koncert = await _context.Koncerti.FindAsync(id);
            var rezervacijaDvorane =  await _context.RezervacijaDvorana.Include(m=>m.rezervacija).FirstOrDefaultAsync(m => m.izvodjacId == koncert.izvodjacId);
            _context.Rezervacija.Remove(rezervacijaDvorane.rezervacija);
            _context.RezervacijaDvorana.Remove(rezervacijaDvorane);
            _context.Koncerti.Remove(koncert);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool KoncertExists(int id)
        {
            return _context.Koncerti.Any(e => e.Id == id);
        }
    }
}
