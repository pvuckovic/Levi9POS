﻿using Levi9.POS.Domain.Models;
namespace Levi9.POS.Domain.Common.IProduct
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> SearchProductsAsync(int page, string name, string orderBy, string direction);
        Task<Product> InsertProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<Product> GetProductByGlobalIdAsync(Guid globalId);
    }
}
