using Implementacija.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Implementacija.Services
{
    public interface IRezervacijaManager
    {
        public Task<double> calculatePrice(TipMjesta t, int koncertId);
        public Task<IDictionary<string, double>> GeneratePrices(int koncertId);
        bool ValidateReservation(Dvorana dvorana);
        public bool HasReservation(string izvodjacId);
    }
}
