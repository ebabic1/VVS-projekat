using Implementacija.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Implementacija.Services
{
    public interface IPorukaManager
    {
        public IEnumerable<Poruka> GetAll();
        public string GetUserId();
    }
}
