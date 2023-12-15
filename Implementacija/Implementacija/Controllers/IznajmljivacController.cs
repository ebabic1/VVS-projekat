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
    public class IznajmljivacController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IznajmljivacController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Iznajmljivac
        public async Task<IActionResult> Index()
        {
            return View(await _context.Iznajmljivaci.ToListAsync());
        }

        // GET: Iznajmljivac/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var iznajmljivac = await _context.Iznajmljivaci
                .FirstOrDefaultAsync(m => m.Id == id);
            if (iznajmljivac == null)
            {
                return NotFound();
            }

            return View(iznajmljivac);
        }

        // GET: Iznajmljivac/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Iznajmljivac/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Email")] Iznajmljivac iznajmljivac)
        {
            if (ModelState.IsValid)
            {
                _context.Add(iznajmljivac);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(iznajmljivac);
        }

        // GET: Iznajmljivac/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iznajmljivac = await _context.Iznajmljivaci.FindAsync(id);
            if (iznajmljivac == null)
            {
                return NotFound();
            }
            return View(iznajmljivac);
        }

        // POST: Iznajmljivac/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Email")] Iznajmljivac iznajmljivac)
        {
            if (id != iznajmljivac.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(iznajmljivac);
                await _context.SaveChangesAsync();
                /*try
                {
                    _context.Update(iznajmljivac);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IznajmljivacExists(iznajmljivac.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }*/
                return RedirectToAction(nameof(Index));
            }
            return View(iznajmljivac);
        }

        // GET: Iznajmljivac/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iznajmljivac = await _context.Iznajmljivaci
                .FirstOrDefaultAsync(m => m.Id == id);
            if (iznajmljivac == null)
            {
                return NotFound();
            }

            return View(iznajmljivac);
        }

        // POST: Iznajmljivac/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var iznajmljivac = await _context.Iznajmljivaci.FindAsync(id);
            _context.Iznajmljivaci.Remove(iznajmljivac);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IznajmljivacExists(string id)
        {
            return _context.Iznajmljivaci.Any(e => e.Id == id);
        }
    }
}
