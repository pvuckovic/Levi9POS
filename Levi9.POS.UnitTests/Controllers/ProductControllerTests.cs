﻿using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Models;
using Levi9.POS.WebApi.Controllers;
using Levi9.POS.WebApi.Mappings;
using Levi9.POS.WebApi.Responses;
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
        private ProductController _productController;

        [SetUp]
        public void Setup()
        {
            _productServiceMock = new Mock<IProductService>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            _productController = new ProductController(_productServiceMock.Object, _mapper);

        }
        [Test]
        public async Task GetProductById_ReturnsOkResult_WhenProductExists()
        {
            // Arrange

            var product = new ProductDTO
            {
                Id = 1,
                GlobalId = Guid.NewGuid(),
                Name = "Test Product",
                ProductImageUrl = "https://test.com/product1.jpg",
                AvailableQuantity = 10,
                LastUpdate = "133277539861042364",
                Price = 99.99f
            };

            _productServiceMock.Setup(p => p.GetProductByIdAsync(product.Id))
                                   .ReturnsAsync(product);

            // Act
            var result = await _productController.GetProductById(product.Id);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var response = ((OkObjectResult)result).Value as ProductResponse;
            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response.Id, Is.EqualTo(product.Id));
                Assert.That(response.GlobalId, Is.EqualTo(product.GlobalId));
                Assert.That(response.Name, Is.EqualTo(product.Name));
                Assert.That(response.ProductImageUrl, Is.EqualTo(product.ProductImageUrl));
                Assert.That(response.AvailableQuantity, Is.EqualTo(product.AvailableQuantity));
                Assert.That(response.LastUpdate, Is.EqualTo(product.LastUpdate));
                Assert.That(response.Price, Is.EqualTo(product.Price));
            });
        }

        [Test]
        public async Task GetProductById_ReturnsNotFoundResult_WhenProductDoesNotExist()
        {// Arrange
            int productId = 1;
            _productServiceMock.Setup(x => x.GetProductByIdAsync(productId)).ReturnsAsync(null as ProductDTO);

            // Act
            var result = await _productController.GetProductById(productId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
            var notFoundResult = (NotFoundResult)result;
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
        }

    }
}