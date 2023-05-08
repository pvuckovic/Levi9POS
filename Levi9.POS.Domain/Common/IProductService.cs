using Levi9.POS.Domain.DTOs;

namespace Levi9.POS.Domain.Common
{
    public interface IProductService
    {
        Task<ProductDTO> GetProductByIdAsync(int id);
    }

}
