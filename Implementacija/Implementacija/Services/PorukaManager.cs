using Implementacija.Data;
using Implementacija.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Implementacija.Controllers;

namespace Implementacija.Services
{
    public class PorukaManager : IPorukaManager
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _contextAccessor;
        public PorukaManager(ApplicationDbContext db, IHttpContextAccessor contextAccessor)
        {
            _dbContext=db;
            _contextAccessor=contextAccessor;
        }
        //dobavlja id od korisnika
        public string GetUserId()
        {
            return _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        //dobavlja sve poruke datog korisnika
        public IEnumerable<Poruka> GetAll()
        {
            return _db.Poruke.Where(poruka => poruka.primalacId == GetUserId()).OrderByDescending(poruka => poruka.Id);
        }
    }
}
