﻿using AutoMapper;
using Levi9.POS.Domain.Common.IProduct;
using Levi9.POS.Domain.DTOs.ProductDTOs;
using Levi9.POS.Domain.Models;
using Levi9.POS.Domain.Services;
using Levi9.POS.WebApi.Mapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
namespace Levi9.POS.UnitTests.Services
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<ILogger<ProductService>> _loggerMock;
        private IMapper _mapper;
        private IProductService _productService;

        [SetUp]
        public void Setup()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _loggerMock = new Mock<ILogger<ProductService>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<ProductMappingProfile>()).CreateMapper();
            _productService = new ProductService(_productRepositoryMock.Object, _loggerMock.Object, _mapper);
        }
        #region GetProductByIdAsync Tests
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
        #endregion
        #region SearchProductsAsync Tests
        [Test]
        public async Task SearchProductsAsync_ReturnsListOfProductDTOs()
        {
            // Arrange
            var requestDTO = new ProductSearchRequestDTO
            {
                Page = 1,
                Name = "search query",
                OrderBy = "name",
                Direction = "asc"
            };
            var expectedProducts = new List<Product>
            {
                new Product {
                    Id = 1,
                    GlobalId = Guid.NewGuid(),
                    Name = "Test Product 1",
                    ProductImageUrl = "https://example.com/product1.jpg",
                    AvailableQuantity = 10,
                    LastUpdate = "133277539861042364",
                    Price = 9.99f
                },
                new Product {
                    Id = 2,
                    GlobalId = Guid.NewGuid(),
                    Name = "Test Product 2",
                    ProductImageUrl = "https://example.com/product2.jpg",
                    AvailableQuantity = 10,
                    LastUpdate = "133277539861042364",
                    Price = 8.88f
                }
            };
            _productRepositoryMock.Setup(x => x.SearchProductsAsync(requestDTO.Page, requestDTO.Name, requestDTO.OrderBy, requestDTO.Direction))
                                  .ReturnsAsync(expectedProducts);
            var expectedProductDTOs = new List<ProductDTO>
            {
                new ProductDTO {
                    Id = 1,
                    GlobalId = Guid.NewGuid(),
                    Name = "Test Product 1",
                    ProductImageUrl = "https://example.com/product1.jpg",
                    AvailableQuantity = 10,
                    LastUpdate = "133277539861042364",
                    Price = 9.99f
                },
                new ProductDTO {
                    Id = 2,
                    GlobalId = Guid.NewGuid(),
                    Name = "Test Product 2",
                    ProductImageUrl = "https://example.com/product2.jpg",
                    AvailableQuantity = 10,
                    LastUpdate = "133277539861042364",
                    Price = 8.88f
                }
            };

            // Act
            var result = await _productService.SearchProductsAsync(requestDTO);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IEnumerable<ProductDTO>>());
            Assert.That(result.Count(), Is.EqualTo(expectedProductDTOs.Count));
            Assert.That(result.First().Name, Is.EqualTo(expectedProductDTOs.First().Name));
            Assert.That(result.Last().Price, Is.EqualTo(expectedProductDTOs.Last().Price));
        }
        [Test]
        public async Task SearchProductsAsync_Returns_ProductDTOs()
        {
            // Arrange
            var requestDTO = new ProductSearchRequestDTO
            {
                Page = 1,
                Name = "test",
                OrderBy = "price",
                Direction = "asc"
            };

            var products = new List<Product>
            {
                new Product {
                    Id = 1,
                    GlobalId = Guid.NewGuid(),
                    Name = "Test Product 1",
                    ProductImageUrl = "https://example.com/product1.jpg",
                    AvailableQuantity = 10,
                    LastUpdate = "133277539861042364",
                    Price = 9.99f
                },
                new Product {
                    Id = 2,
                    GlobalId = Guid.NewGuid(),
                    Name = "Test Product 2",
                    ProductImageUrl = "https://example.com/product2.jpg",
                    AvailableQuantity = 10,
                    LastUpdate = "133277539861042364",
                    Price = 8.88f
                }
            };

            _productRepositoryMock
                .Setup(x => x.SearchProductsAsync(requestDTO.Page, requestDTO.Name, requestDTO.OrderBy, requestDTO.Direction))
                .ReturnsAsync(products);

            // Act
            var result = await _productService.SearchProductsAsync(requestDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<IEnumerable<ProductDTO>>());
            Assert.That(result.Count(), Is.EqualTo(products.Count));
            Assert.That(result.ElementAt(0).Id, Is.EqualTo(products[0].Id));
            Assert.That(result.ElementAt(0).Name, Is.EqualTo(products[0].Name));
            Assert.That(result.ElementAt(0).Price, Is.EqualTo(products[0].Price));
            Assert.That(result.ElementAt(1).Id, Is.EqualTo(products[1].Id));
            Assert.That(result.ElementAt(1).Name, Is.EqualTo(products[1].Name));
            Assert.That(result.ElementAt(1).Price, Is.EqualTo(products[1].Price));
        }
        [Test]
        public async Task SearchProductsAsync_Returns_Empty_List_When_No_Products_Found()
        {
            // Arrange
            var requestDTO = new ProductSearchRequestDTO
            {
                Page = 1,
                Name = "non-existing-product",
                OrderBy = "price",
                Direction = "asc"
            };

            var products = new List<Product>();

            _productRepositoryMock.Setup(x => x.SearchProductsAsync(requestDTO.Page, requestDTO.Name, requestDTO.OrderBy, requestDTO.Direction))
                .ReturnsAsync(products);

            // Act
            var result = await _productService.SearchProductsAsync(requestDTO);

            // Assert
            Assert.That(result, Is.InstanceOf<IEnumerable<ProductDTO>>());
            Assert.That(result.Any(), Is.False);
        }
        #endregion
        #region InsertProductAsync Tests
        [Test]
        public async Task InsertProductAsync_ValidProduct_ReturnsCreatedProduct()
        {
            // Arrange
            var globalIdTest = Guid.NewGuid();
            var insertRequest = new ProductInsertRequestDTO
            {
                GlobalId = globalIdTest,
                Name = "Test Product",
                ProductImageUrl = "https://example.com/test.jpg",
                AvailableQuantity = 10,
                LastUpdate = "133277539861042364",
                Price = 19.99f
            };
            var expectedProduct = new Product
            {
                Id = 1,
                GlobalId = globalIdTest,
                Name = "Test Product",
                ProductImageUrl = "https://example.com/test.jpg",
                AvailableQuantity = 10,
                LastUpdate = "133277539861042364",
                Price = 19.99f
            };
            _productRepositoryMock.Setup(r => r.InsertProductAsync(It.IsAny<Product>())).ReturnsAsync(expectedProduct);

            // Act
            var result = await _productService.InsertProductAsync(insertRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ProductDTO>());
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(expectedProduct.Id));
                Assert.That(result.Name, Is.EqualTo(expectedProduct.Name));
                Assert.That(result.ProductImageUrl, Is.EqualTo(expectedProduct.ProductImageUrl));
                Assert.That(result.AvailableQuantity, Is.EqualTo(expectedProduct.AvailableQuantity));
                Assert.That(result.LastUpdate, Is.EqualTo(expectedProduct.LastUpdate));
                Assert.That(result.Price, Is.EqualTo(expectedProduct.Price));
            });
        }
        #endregion
        #region UpdateProductAsync Tests
        [Test]
        public async Task UpdateProductAsync_ShouldReturnNull_WhenProductNotFound()
        {
            // Arrange
            var productUpdateRequestDto = new ProductUpdateRequestDTO
            {
                GlobalId = Guid.NewGuid(),
                Name = "Test Product 1",
                ProductImageUrl = "https://example.com/product1.jpg",
                AvailableQuantity = 10,
                LastUpdate = "133277539861042364",
                Price = 9.99f
            };
            _productRepositoryMock.Setup(x => x.GetProductByGlobalIdAsync(productUpdateRequestDto.GlobalId)).ReturnsAsync(null as Product);

            // Act
            var result = await _productService.UpdateProductAsync(productUpdateRequestDto);

            // Assert
            Assert.That(result, Is.Null);
        }
        [Test]
        public async Task UpdateProductAsync_ValidProduct_ReturnsUpdatedProduct()
        {
            // Arrange
            var globalId = Guid.NewGuid();
            var productToUpdate = new ProductUpdateRequestDTO
            {
                GlobalId = globalId,
                Name = "New Product Name",
                ProductImageUrl = "https://test123.com/test-new.jpg",
                AvailableQuantity = 10,
                LastUpdate = "133277539861042364",
                Price = 99.99f
            };
            var existingProduct = new Product
            {
                GlobalId = globalId,
                Name = "Old Product Name",
                ProductImageUrl = "https://test123.com/test-old.jpg",
                AvailableQuantity = 15,
                LastUpdate = "133277539861042364",
                Price = 50.00f
            };
            _productRepositoryMock.Setup(x => x.GetProductByGlobalIdAsync(globalId))
                .ReturnsAsync(existingProduct);
            _productRepositoryMock.Setup(x => x.UpdateProductAsync(existingProduct))
                .ReturnsAsync(new Product
                {
                    GlobalId = globalId,
                    Name = productToUpdate.Name,
                    ProductImageUrl = productToUpdate.ProductImageUrl,
                    AvailableQuantity = productToUpdate.AvailableQuantity,
                    LastUpdate = productToUpdate.LastUpdate,
                    Price = productToUpdate.Price
                });

            // Act
            var result = await _productService.UpdateProductAsync(productToUpdate);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.GlobalId, Is.EqualTo(globalId));
                Assert.That(result.Name, Is.EqualTo(productToUpdate.Name));
                Assert.That(result.ProductImageUrl, Is.EqualTo(productToUpdate.ProductImageUrl));
                Assert.That(result.AvailableQuantity, Is.EqualTo(productToUpdate.AvailableQuantity));
                Assert.That(result.Price, Is.EqualTo(productToUpdate.Price));
            });
        }
        #endregion
        #region GetProductByGlobalId Tests
        [Test]
        public async Task GetProductByGlobalIdAsync_ShouldReturnNull_WhenProductNotFound()
        {
            // Arrange
            var globalId = Guid.NewGuid();
            _productRepositoryMock.Setup(x => x.GetProductByGlobalIdAsync(globalId)).ReturnsAsync(null as Product);

            // Act
            var result = await _productService.GetProductByGlobalIdAsync(globalId);

            // Assert
            Assert.That(result, Is.Null);
        }
        [Test]
        public async Task GetProductByGlobalIdAsync_ShouldReturnProduct_WhenProductFound()
        {
            // Arrange
            var globalId = Guid.NewGuid();
            var product = new Product
            {
                Id = 1,
                GlobalId = globalId,
                Name = "Test Product",
                ProductImageUrl = "https://example.com/product.jpg",
                AvailableQuantity = 10,
                LastUpdate = "133277539861042364",
                Price = 19.99f
            };
            _productRepositoryMock.Setup(x => x.GetProductByGlobalIdAsync(globalId)).ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByGlobalIdAsync(globalId);

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
        #endregion
    }
}
