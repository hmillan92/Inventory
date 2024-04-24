using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;

namespace WebServer.Data.Services
{
    public class RateServices : IRateServices
    {
        private readonly DataContext _context;

        public RateServices(DataContext context)
        {
            _context = context;
        }

        public async Task<Rate?> GetRate(int id)
        {
            await _context.Database.EnsureCreatedAsync();

            return await _context.Rates
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Rate>> GetAllRates()
        {
            await _context.Database.EnsureCreatedAsync();

            return await _context.Rates
            .ToListAsync();
        }

        public async Task<string> SaveRate(Rate rate)
        {
            try
            {
                string message = "";

                await _context.Database.EnsureCreatedAsync();

                if (rate.Id == 0)
                {
                    await _context.Rates.AddAsync(rate);
                    message = "Tasa agregada con éxito.";
                }

                else
                {
                    _context.Rates.Update(rate);
                    message = "Tasa actualizada con éxito.";
                }

                await _context.SaveChangesAsync();

                return message;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }         
        }

        public async Task<string> DeleteRate(int id)
        {
            try
            {
                await _context.Database.EnsureCreatedAsync();

                var rate = await _context.Rates.FindAsync(id);

                if (rate == null)
                {
                    return "Error: Tasa no encontrada.";
                }

                _context.Remove(rate);
                await _context.SaveChangesAsync();

                return "Tasa eliminada con éxito.";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }           
        }
    }
}
