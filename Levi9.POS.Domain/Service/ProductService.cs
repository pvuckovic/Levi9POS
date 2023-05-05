using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levi9.POS.Domain.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return null;
            }

            var productDTO = new ProductDTO
            {
                Id = product.Id,
                GlobalId = product.GlobalId,
                Name = product.Name,
                ProductImageUrl = product.ProductImageUrl,
                AvailableQuantity = product.AvailableQuantity,
                LastUpdate = product.LastUpdate,
                Price = product.Price
            };

            return productDTO;
        }
    }
}
