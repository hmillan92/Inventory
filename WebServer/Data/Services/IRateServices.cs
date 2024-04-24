using Web.Models;

namespace WebServer.Data.Services
{
    public interface IRateServices
    {
        public Task<Rate?> GetRate(int id);

        public Task<List<Rate>> GetAllRates();

        public Task<string> SaveRate(Rate rate);

        Task<string> DeleteRate(int id);
    }
}
