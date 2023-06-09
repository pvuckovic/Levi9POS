﻿using Levi9.POS.Domain.Common.IProduct;
using Levi9.POS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Levi9.POS.Domain.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataBaseContext _dataBaseContext;
        private readonly ILogger<ProductRepository> _logger;
        public ProductRepository(DataBaseContext dataBaseContext, ILogger<ProductRepository> logger)
        {
            _dataBaseContext = dataBaseContext;
            _logger = logger;
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            _logger.LogInformation("Entering {FunctionName} in ProductRepository. Timestamp: {Timestamp}.", nameof(GetProductByIdAsync), DateTime.UtcNow);
            var result = await _dataBaseContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            _logger.LogInformation("Retrieving confirmation of product with ID: {Id} in {FunctionName} of ProductRepository. Timestamp: {Timestamp}.", id, nameof(GetProductByIdAsync), DateTime.UtcNow);
            return result;
        }
        public async Task<IEnumerable<Product>> SearchProductsAsync(int page, string name, string orderBy, string direction)
        {
            _logger.LogInformation("Entering {FunctionName} in ProductRepository. Timestamp: {Timestamp}.", nameof(SearchProductsAsync), DateTime.UtcNow);
            IQueryable<Product> query = _dataBaseContext.Products;

            // Filter by name
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.ToLower().Contains(name.ToLower()));
            }

            // Order by
            switch (orderBy?.ToLower())
            {
                case "name":
                    query = direction?.ToLower() == "dsc" ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                    break;
                case "id":
                    query = direction?.ToLower() == "dsc" ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id);
                    break;
                case "globalid":
                    query = direction?.ToLower() == "dsc" ? query.OrderByDescending(p => p.GlobalId) : query.OrderBy(p => p.GlobalId);
                    break;
                case "availablequantity":
                    query = direction?.ToLower() == "dsc" ? query.OrderByDescending(p => p.AvailableQuantity) : query.OrderBy(p => p.AvailableQuantity);
                    break;
                default:
                    query = direction?.ToLower() == "dsc" ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                    break;
            }

            // Paginate
            int pageSize = 10;
            int skip = (page - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);
            var result = await query.ToListAsync();
            _logger.LogInformation("Products retrieved in {FunctionName} of ProductRepository. Timestamp: {Timestamp}.", nameof(SearchProductsAsync), DateTime.UtcNow);
            return result;
        }
        public async Task<bool> DoesProductExist(int productId, string name)
        {
            _logger.LogInformation("Entering {FunctionName} in ProductRepository. Timestamp: {Timestamp}.", nameof(DoesProductExist), DateTime.UtcNow);
            var product = await _dataBaseContext.Products.FirstOrDefaultAsync(c => c.Id == productId && c.Name == name);
            _logger.LogInformation("Retrieving confirmation of product with ID: {Id} and Name: {Name} in {FunctionName} of ProductRepository. Timestamp: {Timestamp}.", productId, name, nameof(DoesProductExist), DateTime.UtcNow);
            return product != null;
        }
        public async Task<Product> InsertProductAsync(Product product)
        {
            _logger.LogInformation("Entering {FunctionName} in ProductRepository. Timestamp: {Timestamp}.", nameof(InsertProductAsync), DateTime.UtcNow);

            // Check if product name already exists
            bool isDuplicate = await _dataBaseContext.Products.AnyAsync(p => p.Name == product.Name);
            if (isDuplicate)
            {
                _logger.LogError("Duplicate product name in {FunctionName} of ProductRepository. Timestamp: {Timestamp}.", nameof(InsertProductAsync), DateTime.UtcNow);
                return null;
            }

            // Add the product to the database
            await _dataBaseContext.Products.AddAsync(product);
            await _dataBaseContext.SaveChangesAsync();

            _logger.LogInformation("Retrieving confirmation of new product in {FunctionName} of ProductRepository. Timestamp: {Timestamp}.", nameof(InsertProductAsync), DateTime.UtcNow);
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            _logger.LogInformation("Entering {FunctionName} in ProductRepository. Timestamp: {Timestamp}.", nameof(UpdateProductAsync), DateTime.UtcNow);
            var existingProduct = await _dataBaseContext.Products.FirstOrDefaultAsync(c => (c.GlobalId == product.GlobalId));
            product.ProductDocuments = await _dataBaseContext.ProductDocuments.Where(pd => pd.ProductId == existingProduct.Id).Include(p => p.Document).ToListAsync();
            product.LastUpdate = DateTime.Now.ToFileTimeUtc().ToString();
            foreach (var article in product.ProductDocuments)
            {
                article.Price = article.Quantity * product.Price;
                article.Document.LastUpdate = product.LastUpdate;
            }
            if (existingProduct != null)
            {
                _logger.LogInformation("Updating product in {FunctionName} of ProductRepository. Timestamp: {Timestamp}.", nameof(UpdateProductAsync), DateTime.UtcNow);
                return await UpdateProduct(existingProduct, product);
            }
            else
            {
                _logger.LogInformation("No updated products in {FunctionName} of ProductRepository. Timestamp: {Timestamp}.", nameof(UpdateProductAsync), DateTime.UtcNow);
                return null;
            }
        }

        private async Task<Product> UpdateProduct(Product contextProduct, Product newProduct)
        {
            _logger.LogInformation("Entering {FunctionName} in ProductRepository. Timestamp: {Timestamp}.", nameof(UpdateProduct), DateTime.UtcNow);
            contextProduct.GlobalId = newProduct.GlobalId;
            contextProduct.Name = newProduct.Name;
            contextProduct.ProductImageUrl = newProduct.ProductImageUrl;
            contextProduct.Price = newProduct.Price;
            contextProduct.AvailableQuantity = newProduct.AvailableQuantity;
            contextProduct.LastUpdate = newProduct.LastUpdate;
            contextProduct.ProductDocuments = newProduct.ProductDocuments;
            await _dataBaseContext.SaveChangesAsync();
            _logger.LogInformation("Retrieving lastUpdate in {FunctionName} of ProductRepository. Timestamp: {Timestamp}.", nameof(UpdateProduct), DateTime.UtcNow);
            return contextProduct;
        }
        public async Task<Product> GetProductByGlobalIdAsync(Guid globalId)
        {
            _logger.LogInformation("Entering {FunctionName} in ProductRepository. Timestamp: {Timestamp}.", nameof(GetProductByGlobalIdAsync), DateTime.UtcNow);
            var result = await _dataBaseContext.Products.FirstOrDefaultAsync(p => p.GlobalId == globalId);
            _logger.LogInformation("Retrieving product with ID: {Id} in {FunctionName} of ProductRepository. Timestamp: {Timestamp}.", globalId, nameof(GetProductByGlobalIdAsync), DateTime.UtcNow);
            return result;
        }
        public async Task<Product> GetProductByNameAsync(string name)
        {
            _logger.LogInformation("Entering {FunctionName} in ProductRepository. Timestamp: {Timestamp}.", nameof(GetProductByNameAsync), DateTime.UtcNow);
            var result = await _dataBaseContext.Products.FirstOrDefaultAsync(p => p.Name == name);
            _logger.LogInformation("Retrieving product with name: {ProductName} in {FunctionName} of ProductRepository. Timestamp: {Timestamp}.", name, nameof(GetProductByNameAsync), DateTime.UtcNow);
            return result;
        }

        public async Task<bool> DoesProductByGlobalIdExists(Guid globalId)
        {
            _logger.LogInformation("Entering { FunctionName} in ProductRepository.Timestamp { Timestamp}.", nameof(DoesProductByGlobalIdExists), DateTime.UtcNow);
            var result = await _dataBaseContext.Products.AnyAsync(p => p.GlobalId == globalId);
            _logger.LogInformation("Retrieving confirmation of product with GlobalId { Id} in { FunctionName}of ProductRepository. Timestamp { Timestamp}.", globalId, nameof(DoesProductByGlobalIdExists), DateTime.UtcNow);
            return result;
        }

        public async Task<bool> DoesProductNameAlreadyExists(Guid globalId, string name)
        {
            _logger.LogInformation("Entering { FunctionName} in ProductRepository.Timestamp { Timestamp}.", nameof(DoesProductNameAlreadyExists), DateTime.UtcNow);
            var result = await _dataBaseContext.Products.AnyAsync(p => p.GlobalId != globalId && p.Name == name);
            _logger.LogInformation("Retrieving confirmation if Name { Name} already exists in { FunctionName} of ProductRepository. Timestamp { Timestamp}.", name, nameof(DoesProductNameAlreadyExists), DateTime.UtcNow);
            return result;
        }
    }
}
