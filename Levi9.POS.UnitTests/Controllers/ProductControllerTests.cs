using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levi9.POS.UnitTests.Controllers
{
    [TestFixture]
    public class ProductControllerTests
    {
        private Mock<IProductService> _productServiceMock;
        private ProductController _productController;

        [SetUp]
        public void Setup()
        {
            _productServiceMock = new Mock<IProductService>();
            _productController = new ProductController(_productServiceMock.Object);
        }
        [Test]
        public async Task GetProductById_ReturnsOkResult_WhenProductExists()
        {
            // Action method and assert that it returns an OKObject
            // Arrange
            var productId = 1;
            var productDTO = new ProductDTO { Id = productId };
            _productServiceMock.Setup(x => x.GetProductByIdAsync(productId)).ReturnsAsync(productDTO);

            // Act
            var result = await _productController.GetProductById(productId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            Assert.IsInstanceOf<ProductDTO>(okResult.Value);
            var returnedProduct = (ProductDTO)okResult.Value;
            Assert.AreEqual(productId, returnedProduct.Id);
        }

        [Test]
        public async Task GetProductById_ReturnsNotFoundResult_WhenProductDoesNotExist()
        {
            //called with the expected productId
            // Arrange
            var productId = 1;
            _productServiceMock.Setup(x => x.GetProductByIdAsync(productId)).ReturnsAsync(null as ProductDTO);

            // Act
            var result = await _productController.GetProductById(productId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

    }
}
