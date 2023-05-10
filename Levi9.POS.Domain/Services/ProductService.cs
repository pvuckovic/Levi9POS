using AutoMapper;
using Levi9.POS.Domain.Common.IProduct;
using Levi9.POS.Domain.DTOs.ProductDTOs;
namespace Levi9.POS.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return null;
            }

            var productDTO = _mapper.Map<ProductDTO>(product);
            return productDTO;
        }
        public async Task<IEnumerable<ProductDTO>> SearchProductsAsync(ProductSearchRequestDTO requestDTO)
        {
            var products = await _productRepository.SearchProductsAsync(requestDTO.Page, requestDTO.Name, requestDTO.OrderBy, requestDTO.Direction);
            var productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return productDTOs;
        }
    }
}
