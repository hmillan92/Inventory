using Web.Models;

namespace WebServer.Data.Services
{
    public interface IProductServices
    {
        Task<Product?> GetProduct(int id);

        Task<Product?> GetProduct(string description);

        Task<List<Product>> GetAllProducts();

        Task<string> SaveProduct(Product product);

        Task<string> DeleteProduct(int id);

        Task<string> FillTable();
    }
}
