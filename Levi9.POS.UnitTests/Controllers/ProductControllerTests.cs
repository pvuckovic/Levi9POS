using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Models;
using Levi9.POS.WebApi.Controllers;
using Levi9.POS.WebApi.Mappings;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Levi9.POS.UnitTests.Controllers
{
    [TestFixture]
    public class ProductControllerTests
    {
        private Mock<IProductService> _productServiceMock;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _productServiceMock = new Mock<IProductService>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
        }
        [Test]
        public async Task GetProductById_ReturnsOkResult_WhenProductExists()
        {
            // Arrange
            int productId = 1;
            var product = new Product
            {
                Id = 1,
                GlobalId = Guid.NewGuid(),
                Name = "Test Product",
                ProductImageUrl = "https://test.com/product1.jpg",
                AvailableQuantity = 10,
                LastUpdate = "133277539861042364",
                Price = 99.99f
            };
            var expectedProductDTO = new ProductDTO
            {
                Id = 1,
                GlobalId = product.GlobalId,
                Name = product.Name,
                ProductImageUrl = product.ProductImageUrl,
                AvailableQuantity = product.AvailableQuantity,
                LastUpdate = product.LastUpdate,
                Price = product.Price
            };
            _productServiceMock.Setup(repo => repo.GetProductByIdAsync(productId))
                .ReturnsAsync(expectedProductDTO);
            var controller = new ProductController(_productServiceMock.Object, _mapper);

            // Act
            var result = await controller.GetProductById(productId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.InstanceOf<ProductDTO>());
            var productDTO = okResult.Value as ProductDTO;
            Assert.Multiple(() =>
            {
                Assert.That(productDTO.Id, Is.EqualTo(expectedProductDTO.Id));
                Assert.That(productDTO.GlobalId, Is.EqualTo(expectedProductDTO.GlobalId));
                Assert.That(productDTO.Name, Is.EqualTo(expectedProductDTO.Name));
                Assert.That(productDTO.ProductImageUrl, Is.EqualTo(expectedProductDTO.ProductImageUrl));
                Assert.That(productDTO.AvailableQuantity, Is.EqualTo(expectedProductDTO.AvailableQuantity));
                Assert.That(productDTO.LastUpdate, Is.EqualTo(expectedProductDTO.LastUpdate));
                Assert.That(productDTO.Price, Is.EqualTo(expectedProductDTO.Price));
            });
        }

        [Test]
        public async Task GetProductById_ReturnsNotFoundResult_WhenProductDoesNotExist()
        {
            // Arrange
            int productId = 1;
            _productServiceMock.Setup(repo => repo.GetProductByIdAsync(productId)).ReturnsAsync(null as ProductDTO);
            var controller = new ProductController(_productServiceMock.Object, _mapper);

            // Act
            var result = await controller.GetProductById(productId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

    }
}
