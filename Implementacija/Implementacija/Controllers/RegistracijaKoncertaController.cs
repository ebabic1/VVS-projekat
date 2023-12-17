using Microsoft.AspNetCore.Mvc;
using Implementacija.Models;
using Implementacija.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Implementacija.Controllers
{
    [Authorize]
    public class RegistracijaKoncertaController : Controller
    {
        private ApplicationDbContext _context;
        public RegistracijaKoncertaController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Register(Koncert koncert)
        {
            _context.Add(koncert);
            await _context.SaveChangesAsync();
            return RedirectToAction("Confirmation");
        }

        public IActionResult Confirmation()
        {
            return View();
        }

    }
}
