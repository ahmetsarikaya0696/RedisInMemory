using RedisExampleApp.API.Models;

namespace RedisExampleApp.API.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        
    }
}
