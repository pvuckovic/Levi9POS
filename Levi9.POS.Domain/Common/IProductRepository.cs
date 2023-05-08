using Levi9.POS.Domain.Models;

namespace Levi9.POS.Domain.Common
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);       
    }
}
