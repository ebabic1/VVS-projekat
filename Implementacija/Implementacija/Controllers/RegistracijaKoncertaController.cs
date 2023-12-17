using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Implementacija.Models;
using Implementacija.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Implementacija.Controllers
{
    [Authorize]
    public class RegistracijaKoncertaController : Controller
    {
        private ApplicationDbContext dd;
        public RegistracijaKoncertaController(ApplicationDbContext ddd)
        {
            dd = ddd;
        }
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Register(Koncert koncert)
        {
            Console.Write(koncert.ToString());
            dd.Add(koncert);
            await dd.SaveChangesAsync();
            return RedirectToAction("Confirmation");
        }

        public IActionResult Confirmation()
        {
            return View();
        }

    }
}
