using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.WebApi.Controllers;
using Levi9.POS.WebApi.Mapper;
using Levi9.POS.WebApi.Request;
using Levi9.POS.WebApi.Response;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

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
        #region GetProductById Tests
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
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.That(notFoundResult.Value, Is.EqualTo($"Product with Id {productId} not found"));
        }
        [Test]
        public async Task GetProductById_ReturnsBadRequest_WhenIdIsZero()
        {
            // Arrange
            int productId = 0;
            var controller = new ProductController(_productServiceMock.Object, _mapper);

            // Act
            var result = await controller.GetProductById(productId);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.That(badRequestResult.Value, Is.EqualTo("Id must be a positive integer"));
        }
        [Test]
        public async Task GetProductById_ReturnsBadRequest_WhenIdIsNegative()
        {
            // Arrange
            int productId = -1;
            var controller = new ProductController(_productServiceMock.Object, _mapper);

            // Act
            var result = await controller.GetProductById(productId);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.That(badRequestResult.Value, Is.EqualTo("Id must be a positive integer"));
        }
        #endregion
        #region SearchProduct Tests
        [Test]
        public async Task SearchProducts_WithValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new ProductSearchRequest
            {
                Page = 1,
                Name = "test",
                OrderBy = "name",
                Direction = "dsc"
            };
            var products = new List<ProductDTO>
            {
                new ProductDTO {
                    Id = 1,
                    GlobalId = Guid.NewGuid(),
                    Name = "Test Product 1",
                    ProductImageUrl = "https://test.com/product1.jpg",
                    AvailableQuantity = 10,
                    LastUpdate = "133277539861042364",
                    Price = 99.99f
                },
                new ProductDTO {
                    Id = 2,
                    GlobalId = Guid.NewGuid(),
                    Name = "Test Product 2",
                    ProductImageUrl = "https://test.com/product2.jpg",
                    AvailableQuantity = 15,
                    LastUpdate = "133277539861042364",
                    Price = 88.88f
                }
            };
            _productServiceMock.Setup(x => x.SearchProductsAsync(It.IsAny<ProductSearchRequestDTO>()))
                .ReturnsAsync(products);
            var expectedResponse = new ProductSearchResponse
            {
                Items = _mapper.Map<IEnumerable<ProductResponse>>(products),
                Page = request.Page
            };

            // Act
            var result = await _productController.SearchProducts(request);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            var actualResponse = (ProductSearchResponse)okResult.Value;
            Assert.That(actualResponse.Items.Count(), Is.EqualTo(expectedResponse.Items.Count()));
            Assert.That(actualResponse.Page, Is.EqualTo(expectedResponse.Page));
            for (int i = 0; i < actualResponse.Items.Count(); i++)
            {
                Assert.That(actualResponse.Items.ElementAt(i).Id, Is.EqualTo(expectedResponse.Items.ElementAt(i).Id));
                Assert.That(actualResponse.Items.ElementAt(i).GlobalId, Is.EqualTo(expectedResponse.Items.ElementAt(i).GlobalId));
                Assert.That(actualResponse.Items.ElementAt(i).Name, Is.EqualTo(expectedResponse.Items.ElementAt(i).Name));
                Assert.That(actualResponse.Items.ElementAt(i).ProductImageUrl, Is.EqualTo(expectedResponse.Items.ElementAt(i).ProductImageUrl));
                Assert.That(actualResponse.Items.ElementAt(i).AvailableQuantity, Is.EqualTo(expectedResponse.Items.ElementAt(i).AvailableQuantity));
                Assert.That(actualResponse.Items.ElementAt(i).LastUpdate, Is.EqualTo(expectedResponse.Items.ElementAt(i).LastUpdate));
                Assert.That(actualResponse.Items.ElementAt(i).Price, Is.EqualTo(expectedResponse.Items.ElementAt(i).Price));
            }
        }
        [Test]
        public async Task SearchProducts_WithNoMatchingProducts_ReturnsNotFound()
        {
            // Arrange
            var requestDTO = new ProductSearchRequestDTO
            {
                Page = 1,
                Name = "Non-existent Product",
                OrderBy = "Name",
                Direction = "asc"
            };
            var request = new ProductSearchRequest
            {
                Page = requestDTO.Page,
                Name = requestDTO.Name,
                OrderBy = requestDTO.OrderBy,
                Direction = requestDTO.Direction
            };

            IEnumerable<ProductDTO> products = Enumerable.Empty<ProductDTO>();

            _productServiceMock.Setup(x => x.SearchProductsAsync(requestDTO))
                .ReturnsAsync(products);

            // Act
            var response = await _productController.SearchProducts(request);

            // Assert
            Assert.That(response, Is.InstanceOf<NotFoundObjectResult>());
            var result = (NotFoundObjectResult)response;
            Assert.That(result.Value, Is.EqualTo("No products were found that match the search criteria."));
        }
        [Test]
        public async Task SearchProducts_WithInvalidPage_ReturnsBadRequest()
        {
            // Arrange
            var request = new ProductSearchRequest { Page = 0 };

            // Act
            var result = await _productController.SearchProducts(request);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }
        #endregion
    }
}
