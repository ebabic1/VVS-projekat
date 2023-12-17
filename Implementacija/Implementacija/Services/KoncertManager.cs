using Implementacija.Data;
using Implementacija.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Implementacija.Services
{
    public class KoncertManager : IKoncertManager
    {
        private readonly ApplicationDbContext _db;
        public KoncertManager(ApplicationDbContext db) => _db = db;
        public async Task<IEnumerable<Koncert>> GetAll() => await _db.Koncerti.ToListAsync();
        public IEnumerable<Koncert> GetRecommended()
        {
            return _db.Koncerti.OrderBy(k => k.zanr).ToList();
        }
        public int GetRemainingSeats(Koncert koncert)
        {
            if (koncert == null) return 0;
            var rezDvorana = _db.RezervacijaDvorana.Where(rez => rez.izvodjacId == koncert.izvodjacId).FirstOrDefault();
            var dvorana = _db.Dvorane.Where(rez => rez.Id == rezDvorana.dvoranaId).FirstOrDefault();
            var count = _db.RezervacijaKarata.Where(rez => rez.koncertId == koncert.Id).Count();
            return dvorana.brojSjedista - count;
        }
        public async Task<IEnumerable<Koncert>> SortAktuelni(string? aktuelniSortOrder, string? searchString)
        {
            var koncerti = await GetAll();
            if (!String.IsNullOrEmpty(searchString))
            {
                koncerti = koncerti.Where(s => s.naziv?.Contains(searchString) == true).ToList();
            }

            switch (aktuelniSortOrder)
            {
                case "name_desc":
                    koncerti = koncerti.OrderByDescending(s => s.naziv).ToList();
                    break;
                case "Date":
                    koncerti = koncerti.OrderBy(s => s.datum).ToList();
                    break;
                case "date_desc":
                    koncerti = koncerti.OrderByDescending(s => s.datum).ToList();
                    break;
                default:
                    koncerti = koncerti.OrderBy(s => s.naziv).ToList();
                    break;
            }

            return koncerti;
        }


    }

}