using Implementacija.Models;
using Implementacija.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Implementacija.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IKoncertManager _koncertManager;
        public HomeController(ILogger<HomeController> logger, IKoncertManager koncertManager)
        {
            _koncertManager = koncertManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string aktuelniSortOrder, string searchString)
        {
           ViewData["NameSortParm"] = String.IsNullOrEmpty(aktuelniSortOrder) ? "name_desc" : "";
           ViewData["DateSortParm"] = aktuelniSortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;
            return View(_koncertManager.SortAktuelni(aktuelniSortOrder, searchString).Result);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
