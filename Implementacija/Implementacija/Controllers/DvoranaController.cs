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
    public class DvoranaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DvoranaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Iznajmljivac, Izvodjac")]
        // GET: Dvorana
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Dvorane.Include(d => d.iznajmljivac);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize(Roles = "Iznajmljivac, Izvodjac")]
        // GET: Dvorana/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dvorana = await _context.Dvorane
                .Include(d => d.iznajmljivac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dvorana == null)
            {
                return NotFound();
            }

            return View(dvorana);
        }

        [Authorize(Roles = "Iznajmljivac")]
        // GET: Dvorana/Create
        public IActionResult Create()
        {
            ViewData["iznajmljivacId"] = new SelectList(_context.Iznajmljivaci, "Id", "Id");
            return View();
        }

        // POST: Dvorana/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,nazivDvorane,adresaDvorane,brojSjedista,iznajmljivacId")] Dvorana dvorana)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dvorana);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["iznajmljivacId"] = new SelectList(_context.Iznajmljivaci, "Id", "Id", dvorana.iznajmljivacId);
            return View(dvorana);
        }

        [Authorize(Roles = "Iznajmljivac")]
        // GET: Dvorana/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dvorana = await _context.Dvorane.FindAsync(id);
            if (dvorana == null)
            {
                return NotFound();
            }
            ViewData["iznajmljivacId"] = new SelectList(_context.Iznajmljivaci, "Id", "Id", dvorana.iznajmljivacId);
            return View(dvorana);
        }

        // POST: Dvorana/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,nazivDvorane,adresaDvorane,brojSjedista,iznajmljivacId")] Dvorana dvorana)
        {
            if (id != dvorana.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dvorana);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DvoranaExists(dvorana.Id))
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
            ViewData["iznajmljivacId"] = new SelectList(_context.Iznajmljivaci, "Id", "Id", dvorana.iznajmljivacId);
            return View(dvorana);
        }

        [Authorize(Roles = "Iznajmljivac")]
        // GET: Dvorana/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dvorana = await _context.Dvorane
                .Include(d => d.iznajmljivac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dvorana == null)
            {
                return NotFound();
            }

            return View(dvorana);
        }

        [Authorize(Roles = "Iznajmljivac")]
        // POST: Dvorana/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dvorana = await _context.Dvorane.FindAsync(id);
            _context.Dvorane.Remove(dvorana);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DvoranaExists(int id)
        {
            return _context.Dvorane.Any(e => e.Id == id);
        }
    }
}
