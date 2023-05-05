using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.Models;
using Levi9.POS.Domain.Service;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levi9.POS.UnitTests.Services
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private ProductService _productService;

        [SetUp]
        public void Setup()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _productService = new ProductService(_productRepositoryMock.Object);
        }

        [Test]
        public async Task GetProductByIdAsync_ReturnsNull_WhenProductNotFound()
        {
            // Arrange
            int id = 1;
            _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(id)).ReturnsAsync((Product)null);

            // Act
            var result = await _productService.GetProductByIdAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetProductByIdAsync_ReturnsProductDTO_WhenProductFound()
        {
            // Arrange
            int id = 1;
            var product = new Product
            {
                Id = id,
                GlobalId = Guid.NewGuid(),
                Name = "Test Product",
                ProductImageUrl = "http://example.com/image.png",
                AvailableQuantity = 10,
                LastUpdate = "20220505",
                Price = 9.99f
            };
            _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(id)).ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(product.Id, result.Id);
            Assert.AreEqual(product.GlobalId, result.GlobalId);
            Assert.AreEqual(product.Name, result.Name);
            Assert.AreEqual(product.ProductImageUrl, result.ProductImageUrl);
            Assert.AreEqual(product.AvailableQuantity, result.AvailableQuantity);
            Assert.AreEqual(product.LastUpdate, result.LastUpdate);
            Assert.AreEqual(product.Price, result.Price);
        }
    }
}
