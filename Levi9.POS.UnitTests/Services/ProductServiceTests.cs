using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Models;
using Levi9.POS.Domain.Service;
using Levi9.POS.WebApi.Mappings;
using Moq;
using NUnit.Framework;

namespace Levi9.POS.UnitTests.Services
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private IMapper _mapper;
        private IProductService _productService;

        [SetUp]
        public void Setup()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
            _productService = new ProductService(_productRepositoryMock.Object, _mapper);
        }

        [Test]
        public async Task GetProductByIdAsync_WithExistingProductId_ReturnsProductDTO()
        {
            // Arrange
            var productId = 1;
            var product = new Product
            {
                Id = productId,
                GlobalId = Guid.NewGuid(),
                Name = "Test Product 1",
                ProductImageUrl = "https://example.com/product1.jpg",
                AvailableQuantity = 10,
                LastUpdate = "133277539861042364",
                Price = 9.99f
            };
            _productRepositoryMock.Setup(r => r.GetProductByIdAsync(productId)).ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ProductDTO>());
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(product.Id));
                Assert.That(result.Name, Is.EqualTo(product.Name));
                Assert.That(result.ProductImageUrl, Is.EqualTo(product.ProductImageUrl));
                Assert.That(result.AvailableQuantity, Is.EqualTo(product.AvailableQuantity));
                Assert.That(result.LastUpdate, Is.EqualTo(product.LastUpdate));
                Assert.That(result.Price, Is.EqualTo(product.Price));
            });
        }

        [Test]
        public async Task GetProductByIdAsync_WithNonExistingProductId_ReturnsNull()
        {
            // Arrange
            int productId = 1;
            _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(productId)).ReturnsAsync(null as Product);

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
