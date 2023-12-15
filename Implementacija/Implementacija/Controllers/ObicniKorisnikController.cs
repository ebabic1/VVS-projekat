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

namespace Implementacija.Controllers
{
    [Authorize]
    public class ObicniKorisnikController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ObicniKorisnikController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ObicniKorisniks
        public async Task<IActionResult> Index()
        {
            return View(await _context.ObicniKorisnici.ToListAsync());
        }

        // GET: ObicniKorisniks/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obicniKorisnik = await _context.ObicniKorisnici
                .FirstOrDefaultAsync(m => m.Id == id);
            if (obicniKorisnik == null)
            {
                return NotFound();
            }

            return View(obicniKorisnik);
        }

        // GET: ObicniKorisniks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ObicniKorisniks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Email")] ObicniKorisnik obicniKorisnik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(obicniKorisnik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(obicniKorisnik);
        }

        // GET: ObicniKorisniks/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obicniKorisnik = await _context.ObicniKorisnici.FindAsync(id);
            if (obicniKorisnik == null)
            {
                return NotFound();
            }
            return View(obicniKorisnik);
        }

        // POST: ObicniKorisniks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Email")] ObicniKorisnik obicniKorisnik)
        {
            if (id != obicniKorisnik.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(obicniKorisnik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ObicniKorisnikExists(obicniKorisnik.Id))
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
            return View(obicniKorisnik);
        }

        // GET: ObicniKorisniks/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obicniKorisnik = await _context.ObicniKorisnici
                .FirstOrDefaultAsync(m => m.Id == id);
            if (obicniKorisnik == null)
            {
                return NotFound();
            }

            return View(obicniKorisnik);
        }

        // POST: ObicniKorisniks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var obicniKorisnik = await _context.ObicniKorisnici.FindAsync(id);
            _context.ObicniKorisnici.Remove(obicniKorisnik);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ObicniKorisnikExists(string id)
        {
            return _context.ObicniKorisnici.Any(e => e.Id == id);
        }
    }
}
