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
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Implementacija.Services;

namespace Implementacija.Controllers
{
    [Authorize]
    public class PorukaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPorukaManager _porukaManager;
        public PorukaController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IPorukaManager porukaManager)
        {
            _context = context;
            _userManager = userManager;
            _porukaManager = porukaManager;
        }

        // GET: Poruka
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Poruke.Include(p => p.primalac);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Poruka/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poruka = await _context.Poruke
                .Include(p => p.primalac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (poruka == null)
            {
                return NotFound();
            }

            return View(poruka);
        }

        // GET: Poruka/Create
        public IActionResult Create()
        {
            // Sada se umjesto primalacId u Create formi prikazuju mejlovi, kasnije se u Create metodi ovaj mejl prevodi opet u Id
            // Ovo je uradjeno da bi pri odabiru primaoca birali mejl, a ne Id
            // Where je da ne bi prikazalo ulogovanog korisnika u listi primaoca
            ViewData["primalacId"] = new SelectList(_context.ObicniKorisnici.Where(o => o.Id != _porukaManager.GetUserId()), "Email", "Email"); ;
            return View();
        }

        // POST: Poruka/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,sadrzaj,primalacId")] Poruka poruka)
        {
            if (ModelState.IsValid)
            {
                _context.Add(poruka);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["primalacId"] = new SelectList(_context.ObicniKorisnici, "Id", "Id", poruka.primalacId);
            return View(poruka);
        }

        // GET: Poruka/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poruka = await _context.Poruke.FindAsync(id);
            if (poruka == null)
            {
                return NotFound();
            }
            ViewData["primalacId"] = new SelectList(_context.ObicniKorisnici, "Id", "Id", poruka.primalacId);
            return View(poruka);
        }

        // POST: Poruka/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,sadrzaj,primalacId")] Poruka poruka)
        {
            if (id != poruka.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(poruka);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["primalacId"] = new SelectList(_context.ObicniKorisnici, "Id", "Id", poruka.primalacId);
            return View(poruka);
        }
        // GET: Poruka/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poruka = await _context.Poruke
                .Include(p => p.primalac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (poruka == null)
            {
                return NotFound();
            }

            return View(poruka);
        }

        // POST: Poruka/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var poruka = await _context.Poruke.FindAsync(id);
            _context.Poruke.Remove(poruka);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PorukaExists(int id)
        {
            return _context.Poruke.Any(e => e.Id == id);
        }
    }
}
