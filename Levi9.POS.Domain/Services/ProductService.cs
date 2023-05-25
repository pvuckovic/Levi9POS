using AutoMapper;
using Levi9.POS.Domain.Common.IProduct;
using Levi9.POS.Domain.DTOs.ProductDTOs;
using Levi9.POS.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Levi9.POS.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger, IMapper mapper)
        {
            _productRepository = productRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            _logger.LogInformation("Entering {FunctionName} in ProductService. Timestamp: {Timestamp}.", nameof(GetProductByIdAsync), DateTime.UtcNow);
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product not found with ID: {ProductId} in {FunctionName} of ProductService. Timestamp: {Timestamp}.", id, nameof(GetProductByIdAsync), DateTime.UtcNow);
                return null;
            }

            var productDTO = _mapper.Map<ProductDTO>(product);
            _logger.LogInformation("Product retrieved successfully with ID: {ProductId} in {FunctionName} of ProductService. Timestamp: {Timestamp}.", id, nameof(GetProductByIdAsync), DateTime.UtcNow);
            return productDTO;
        }
        public async Task<IEnumerable<ProductDTO>> SearchProductsAsync(ProductSearchRequestDTO requestDTO)
        {
            _logger.LogInformation("Entering {FunctionName} in ProductService. Timestamp: {Timestamp}.", nameof(SearchProductsAsync), DateTime.UtcNow);
            var products = await _productRepository.SearchProductsAsync(requestDTO.Page, requestDTO.Name, requestDTO.OrderBy, requestDTO.Direction);
            var productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(products);
            _logger.LogInformation("Products retrieved in {FunctionName} of ProductService. Timestamp: {Timestamp}.", nameof(SearchProductsAsync), DateTime.UtcNow);
            return productDTOs;
        }
        public async Task<ProductDTO> InsertProductAsync(ProductInsertRequestDTO product)
        {
            _logger.LogInformation("Entering {FunctionName} in ProductService. Timestamp: {Timestamp}.", nameof(InsertProductAsync), DateTime.UtcNow);
            var productEntity = _mapper.Map<Product>(product);
            var insertedProduct = await _productRepository.InsertProductAsync(productEntity);
            var insertedProductDTO = _mapper.Map<ProductDTO>(insertedProduct);
            _logger.LogInformation("Retrieving confirmation of new product in {FunctionName} of ProductService. Timestamp: {Timestamp}.", nameof(InsertProductAsync), DateTime.UtcNow);
            return insertedProductDTO;
        }

        public async Task<ProductDTO> UpdateProductAsync(ProductUpdateRequestDTO product)
        {
            _logger.LogInformation("Entering {FunctionName} in ProductService. Timestamp: {Timestamp}.", nameof(UpdateProductAsync), DateTime.UtcNow);

            var entity = await _productRepository.GetProductByGlobalIdAsync(product.GlobalId);
            if (entity == null)
            {
                _logger.LogError("Product not found with GlobalId: {GlobalId} in {FunctionName} of ProductService. Timestamp: {Timestamp}.", product.GlobalId, nameof(UpdateProductAsync), DateTime.UtcNow);
                return null;
            }
            _mapper.Map(product, entity);
            var updatedProduct = await _productRepository.UpdateProductAsync(entity);
            _logger.LogInformation("Retrieving confirmation of updated product in {FunctionName} of ProductService. Timestamp: {Timestamp}.", nameof(UpdateProductAsync), DateTime.UtcNow);
            return _mapper.Map<ProductDTO>(updatedProduct);
        }
        public async Task<ProductDTO> GetProductByGlobalIdAsync(Guid globalId)
        {
            _logger.LogInformation("Entering {FunctionName} in ProductService. Timestamp: {Timestamp}.", nameof(GetProductByGlobalIdAsync), DateTime.UtcNow);

            var entity = await _productRepository.GetProductByGlobalIdAsync(globalId);
            if (entity == null)
            {
                _logger.LogError("Product not found with GlobalId: {GlobalId} in {FunctionName} of ProductService. Timestamp: {Timestamp}.", globalId, nameof(GetProductByGlobalIdAsync), DateTime.UtcNow);
                return null;
            }
            _logger.LogInformation("Product retrieved successfully with GlobalID: {GlobalId} in {FunctionName} of ProductService. Timestamp: {Timestamp}.", globalId, nameof(GetProductByGlobalIdAsync), DateTime.UtcNow);
            return _mapper.Map<ProductDTO>(entity);
        }

        public async Task<string> SyncProducts(List<ProductSyncRequestDTO> products)
        {
            _logger.LogInformation("Entering {FunctionName} in ProductService. Timestamp: {Timestamp}.", nameof(SyncProducts), DateTime.UtcNow);
            foreach (var product in products)
            {
                if (await _productRepository.DoesProductNameAlreadyExists(product.GlobalId, product.Name))
                {
                    _logger.LogError("Product name: {Name} already exists for GlobalId: {GlobalId} in {FunctionName} of ProductService. Timestamp: {Timestamp}.", product.Name, product.GlobalId, nameof(GetProductByGlobalIdAsync), DateTime.UtcNow);
                    return null;
                }
            }

            var mapedProducts = _mapper.Map<List<Product>>(products);
            string lastUpdate = null;
            foreach (var product in mapedProducts)  
            {
                lastUpdate = DateTime.Now.ToFileTimeUtc().ToString();
                product.LastUpdate = lastUpdate;
                if (await _productRepository.DoesProductByGlobalIdExists(product.GlobalId))
                {
                    var existingProduct = await _productRepository.GetProductByGlobalIdAsync(product.GlobalId);
                    product.Id = existingProduct.Id;
                    await _productRepository.UpdateProductAsync(product);
                    _logger.LogInformation("Product updated successfully in {FunctionName} of ProductService. Timestamp: {Timestamp}.", nameof(SyncProducts), DateTime.UtcNow);
                }
                else
                {
                    _logger.LogInformation("Product inserted successfully in {FunctionName} of ProductService. Timestamp: {Timestamp}.", nameof(SyncProducts), DateTime.UtcNow);
                    await _productRepository.InsertProductAsync(product);
                }
            }
            return lastUpdate;
        }
    }
}
