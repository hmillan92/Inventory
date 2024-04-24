using Microsoft.EntityFrameworkCore;
using SpreadsheetLight;
using Web.Data;
using Web.Models;

namespace WebServer.Data.Services
{
    public class ProductServices : IProductServices
    {
        private readonly DataContext _context;

        public ProductServices(DataContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetProduct(int id)
        {
            await _context.Database.EnsureCreatedAsync();

            return await _context.Products
                .Include(r => r.Rate)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product?> GetProduct(string description)
        {
            await _context.Database.EnsureCreatedAsync();

            return await _context.Products
                .Include(r => r.Rate)
                .FirstOrDefaultAsync(p => p.Description == description);
        }

        public async Task<List<Product>> GetAllProducts()
        {
            await _context.Database.EnsureCreatedAsync();

            return await _context.Products
            .Include(r => r.Rate)
            .OrderBy(p => p.Description)
            .ToListAsync();
        }

        public async Task<string> SaveProduct(Product product)
        {
            try
            {
                string message = "";

                await _context.Database.EnsureCreatedAsync();

                product.Rate = await _context.Rates.FirstAsync();

                if (product.Rate == null)
                {
                    return "Error: no se encontró la Tasa.";
                }

                if (product.Id == 0)
                {
                    await _context.Products.AddAsync(product);
                    message = "Producto agregado con éxito.";
                }

                else
                {
                    _context.Products.Update(product);
                    message = "Producto actualizado con éxito.";
                }

                await _context.SaveChangesAsync();
                return message;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public async Task<string> DeleteProduct(int id)
        {
            try
            {
                await _context.Database.EnsureCreatedAsync();

                var product = await GetProduct(id);

                if (product == null)
                {
                    return "Error: El producto que intenta eliminar no existe.";
                }

                _context.Remove(product);
                await _context.SaveChangesAsync();

                return "Producto eliminado con éxito.";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }          
        }

        public async Task<string> FillTable()
        {
            string path = @"C:\PlantillaProductos.xlsx";

            try
            {
                SLDocument sl = new SLDocument(path, "Base");

                int iRow = 2;

                while (!string.IsNullOrEmpty(sl.GetCellValueAsString(iRow, 1)))
                {
                    string productName = sl.GetCellValueAsString(iRow, 1);
                    string price = sl.GetCellValueAsString(iRow, 2);
                    decimal decimalPrice = 0;

                    bool parse = decimal.TryParse(price,out decimalPrice);

                    iRow++;

                    var existProduct = await GetProduct(productName);

                    _ = await SaveProduct(new Product
                    {
                        Id = existProduct == null ? 0 : existProduct.Id,
                        Description = productName,
                        DollarPrice = decimalPrice,
                    });
                }

                return "Se han guardado los registros con éxito.";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}