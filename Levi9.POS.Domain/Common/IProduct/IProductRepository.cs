using Levi9.POS.Domain.Models;
namespace Levi9.POS.Domain.Common.IProduct
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> SearchProductsAsync(int page, string name, string orderBy, string direction);
        Task<Product> InsertProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<Product> GetProductByGlobalIdAsync(Guid globalId);
        public Task<bool> DoesProductExist(int productId, string name);
        Task<Product> GetProductByNameAsync(string name);
        public Task<bool> DoesProductByGlobalIdExists(Guid globalId);

        public Task<bool> DoesProductNameAlreadyExists(Guid globalId, string name);
    }
}
