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

namespace Implementacija.Services
{
    public class PorukaManager : IPorukaManager
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _contextAccessor;
        public PorukaManager(ApplicationDbContext db, IHttpContextAccessor contextAccessor)
        {
            _db=db;
            _contextAccessor=contextAccessor;
        }
        public string GetUserId()
        {
            var x = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return x;
        }
        public IEnumerable<Poruka> GetAll()
        {
            return _db.Poruke.Where(poruka => poruka.primalacId == GetUserId()).OrderByDescending(poruka => poruka.Id);
        }
    }
}
