using Levi9.POS.Domain.DTOs.ProductDTOs;
namespace Levi9.POS.Domain.Common.IProduct
{
    public interface IProductService
    {
        Task<ProductDTO> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDTO>> SearchProductsAsync(ProductSearchRequestDTO request);
        Task<ProductDTO> InsertProductAsync(ProductInsertRequestDTO product);
        Task<ProductDTO> UpdateProductAsync(ProductUpdateRequestDTO product);
        Task<ProductDTO> GetProductByGlobalIdAsync(Guid globalId);
    }
}
