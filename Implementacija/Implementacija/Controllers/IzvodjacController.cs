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
    public class IzvodjacController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IzvodjacController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Izvodjac
        public async Task<IActionResult> Index()
        {
            return View(await _context.Izvodjaci.ToListAsync());
        }

        // GET: Izvodjac/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izvodjac = await _context.Izvodjaci
                .FirstOrDefaultAsync(m => m.Id == id);
            if (izvodjac == null)
            {
                return NotFound();
            }

            return View(izvodjac);
        }

        // GET: Izvodjac/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Izvodjac/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Email")] Izvodjac izvodjac)
        {
            if (ModelState.IsValid)
            {
                _context.Add(izvodjac);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(izvodjac);
        }

        // GET: Izvodjac/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izvodjac = await _context.Izvodjaci.FindAsync(id);
            if (izvodjac == null)
            {
                return NotFound();
            }
            return View(izvodjac);
        }

        // POST: Izvodjac/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Email")] Izvodjac izvodjac)
        {
            if (id != izvodjac.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(izvodjac);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IzvodjacExists(izvodjac.Id))
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
            return View(izvodjac);
        }

        // GET: Izvodjac/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izvodjac = await _context.Izvodjaci
                .FirstOrDefaultAsync(m => m.Id == id);
            if (izvodjac == null)
            {
                return NotFound();
            }

            return View(izvodjac);
        }

        // POST: Izvodjac/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var izvodjac = await _context.Izvodjaci.FindAsync(id);
            _context.Izvodjaci.Remove(izvodjac);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IzvodjacExists(string id)
        {
            return _context.Izvodjaci.Any(e => e.Id == id);
        }
    }
}
