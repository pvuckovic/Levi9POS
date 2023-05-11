using AutoMapper;
using Levi9.POS.Domain.Common.IProduct;
using Levi9.POS.Domain.DTOs.ProductDTOs;
using Levi9.POS.Domain.Models;

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
        public async Task<ProductDTO> InsertProductAsync(ProductInsertRequestDTO product)
        {
            var productEntity = _mapper.Map<Product>(product);
            var insertedProduct = await _productRepository.InsertProductAsync(productEntity);
            var insertedProductDTO = _mapper.Map<ProductDTO>(insertedProduct);

            return insertedProductDTO;
        }
        public async Task<ProductDTO> UpdateProductAsync(ProductUpdateRequestDTO product)
        {
            var entity = await _productRepository.GetProductByGlobalIdAsync(product.GlobalId);
            if (entity == null)
            {
                return null;
            }
            _mapper.Map(product, entity);
            var updatedProduct = await _productRepository.UpdateProductAsync(entity);

            return _mapper.Map<ProductDTO>(updatedProduct);
        }
        public async Task<ProductDTO> GetProductByGlobalIdAsync(Guid globalId)
        {
            var entity = await _productRepository.GetProductByGlobalIdAsync(globalId);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<ProductDTO>(entity);
        }
    }
}
