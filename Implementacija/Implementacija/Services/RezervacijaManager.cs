using Implementacija.Data;
using Implementacija.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Implementacija.Services
{
    public class RezervacijaManager : IRezervacijaManager
    {
        private readonly ApplicationDbContext _db;
        private readonly IPorukaManager _porukaManager;
        public RezervacijaManager(ApplicationDbContext db, IPorukaManager porukaManager)
        {
            _db = db;
            _porukaManager = porukaManager;
        }
        public bool HasReservation(string izvodjacId)
        {
            return _db.RezervacijaDvorana.Any(o => o.izvodjacId == izvodjacId && o.dvorana.iznajmljivac.Id == _porukaManager.GetUserId());
        }
        public async Task<double> calculatePrice(TipMjesta t, int koncertId)
        {
            if (t == TipMjesta.VIP) return 300;
            else if (t == TipMjesta.TRIBINA) return 200;
            else return 100;
        }
        public async Task<IDictionary<string, double>> GeneratePrices (int koncertId)
        {
            IDictionary<string, double> cijene = new Dictionary<string, double>(); 
            foreach (TipMjesta enumValue in Enum.GetValues(typeof(TipMjesta)))
            {
                string enumString = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                ?.GetName();
                cijene[enumString] = await calculatePrice(enumValue, koncertId);
            }
            return cijene;
        }
        // provjeri ima li izvodjac vec rezervaciju
        public bool ValidateReservation(Dvorana d)
        {
            var id = _porukaManager.GetUserId(); 
            var list = new List<RezervacijaDvorane>();
            //nadji sve rezervacije od trenutnog korisnika
            return !_db.RezervacijaDvorana.Any(o => o.izvodjacId == id);
        }
    }
}
