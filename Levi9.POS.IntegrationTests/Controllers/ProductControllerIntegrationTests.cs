using Levi9.POS.Domain;
using Levi9.POS.Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net.Http.Headers;
using System.Net;
using Levi9.POS.WebApi.Request.ProductRequest;
using Levi9.POS.WebApi.Response.ProductResponse;
using Levi9.POS.IntegrationTests.Fixtures;
using System.Text;

namespace Levi9.POS.IntegrationTests.Controllers
{
    [TestFixture]
    public class ProductControllerIntegrationTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private DataBaseContext _dbContext;
        private List<Product> _testProducts;

        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Replace the database context registration with an in-memory database
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<DataBaseContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<DataBaseContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDatabase");
                    });

                    // Build the service provider to resolve and initialize the database context
                    var serviceProvider = services.BuildServiceProvider();

                    // Create a new instance of the database context
                    _dbContext = serviceProvider.GetRequiredService<DataBaseContext>();

                    // Check if the database is empty
                    if (!_dbContext.Products.Any())
                    {
                        // Generate test products
                        _testProducts = ProductFixture.CreateTestProducts();
                        _dbContext.Products.AddRange(_testProducts);
                        _dbContext.SaveChanges();
                    }
                });
            });
            _client = _factory.CreateClient();
            string token = ProductFixture.GenerateJwt();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        [TearDown]
        public void TearDown()
        {
            // Clean up the in-memory database and dispose of resources
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
            _factory.Dispose();
            _client.Dispose();
        }
        #region GetProductById
        [Test]
        public async Task GetProductById_ValidId_ReturnsOk()
        {
            // Arrange
            var product = _testProducts.First();

            // Act
            var response = await _client.GetAsync($"/v1/Product/{product.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Product>(content);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(product.Id));
                Assert.That(result.GlobalId, Is.EqualTo(product.GlobalId));
                Assert.That(result.Name, Is.EqualTo(product.Name));
                Assert.That(result.ProductImageUrl, Is.EqualTo(product.ProductImageUrl));
                Assert.That(result.AvailableQuantity, Is.EqualTo(product.AvailableQuantity));
                Assert.That(result.LastUpdate, Is.EqualTo(product.LastUpdate));
                Assert.That(result.Price, Is.EqualTo(product.Price));
            });
        }
        [Test]
        public async Task GetProductById_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = -1;

            // Act
            var response = await _client.GetAsync($"/v1/Product/{invalidId}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [Test]
        public async Task GetProductById_ProductNotFound_ReturnsNotFound()
        {
            // Arrange
            int nonExistentId = 30;

            // Act
            var response = await _client.GetAsync($"/v1/Product/{nonExistentId}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        [Test]
        public async Task GetProductById_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            var product = _testProducts.First();

            // Remove the authorization header
            _client.DefaultRequestHeaders.Authorization = null;

            // Act
            var response = await _client.GetAsync($"/v1/Product/{product.Id}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        #endregion
        #region SearchProducts
        [Test]
        public async Task SearchProducts_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new ProductSearchRequest
            {
                Page = 1,
                Name = "Product",
                OrderBy = "id",
                Direction = "asc"
            };

            // Act
            var response = await _client.GetAsync($"/v1/Product?page={request.Page}&name={request.Name}&orderBy={request.OrderBy}&direction={request.Direction}");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProductSearchResponse>(content);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(request.Page, result.Page);
            Assert.That(result.Items, Is.Not.Null);
            Assert.IsInstanceOf<IEnumerable<ProductResponse>>(result.Items);

            foreach (var productResponse in result.Items)
            {
                Assert.IsTrue(productResponse.Name.Contains(request.Name));
                Assert.AreEqual(request.OrderBy, "id");
                Assert.AreEqual(request.Direction, "asc");
            }
        }
        [Test]
        public async Task SearchProducts_InvalidPage_ReturnsBadRequest()
        {
            // Arrange
            var request = new ProductSearchRequest
            {
                Page = -1,
                Name = "T-Shirt",
                OrderBy = "id",
                Direction = "asc"
            };
            // Act
            var response = await _client.GetAsync($"/v1/Product?page={request.Page}&name={request.Name}&orderBy={request.OrderBy}&direction={request.Direction}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            var content = await response.Content.ReadAsStringAsync();
            Assert.That(content, Is.EqualTo("The 'page' parameter must be greater than 0."));
        }
        [Test]
        public async Task SearchProducts_OrderBySpecifiedButDirectionNotSpecified_ReturnsBadRequest()
        {
            // Arrange
            var request = new ProductSearchRequest
            {
                Page = 1,
                Name = "Product",
                OrderBy = "id",
                Direction = ""
            };// Act
            var response = await _client.GetAsync($"/v1/Product?page={request.Page}&name={request.Name}&orderBy={request.OrderBy}&direction={request.Direction}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            var content = await response.Content.ReadAsStringAsync();
            Assert.That(content, Is.EqualTo("If OrderBy is not empty, you must enter Direction!"));
        }
        [Test]
        public async Task SearchProducts_NoProductsFound_ReturnsNotFound()
        {
            // Arrange
            var request = new ProductSearchRequest
            {
                Page = 1,
                Name = "Nonexistent Product",
                OrderBy = "id",
                Direction = "asc"
            };
            // Act
            var response = await _client.GetAsync($"/v1/Product?page={request.Page}&name={request.Name}&orderBy={request.OrderBy}&direction={request.Direction}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            var content = await response.Content.ReadAsStringAsync();
            Assert.That(content, Is.EqualTo("No products were found that match the search criteria."));
        }
        [Test]
        public async Task SearchProducts_UnauthorizedUser_ReturnsUnauthorized()
        {
            // Arrange
            var request = new ProductSearchRequest
            {
                Page = 1,
                Name = "Product",
                OrderBy = "name",
                Direction = "asc"
            };
            _client.DefaultRequestHeaders.Authorization = null;
            // Act
            // Simulate an unauthorized user by not providing the authentication token
            var response = await _client.GetAsync($"/v1/Product?page={request.Page}&name={request.Name}&orderBy={request.OrderBy}&direction={request.Direction}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        #endregion
        #region InsertProduct
        [Test]
        public async Task InsertProduct_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var request = new ProductInsertRequest
            {
                GlobalId = Guid.Parse("025d4482-79e2-4ad1-bbee-94e267572418"),
                Name = "T-Shirt Levi9",
                ProductImageUrl = "[baseURL]/product/025d4482-79e2-4ad1-bbee-94e267572418.png",
                AvailableQuantity = 15000,
                LastUpdate = "634792557112051692",
                Price = 15.23f
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/v1/Product", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            var responseContent = await response.Content.ReadAsStringAsync();
            var productResponse = JsonConvert.DeserializeObject<ProductInsertResponse>(responseContent);
            Assert.That(productResponse.Id, Is.GreaterThan(0));
        }
        [Test]
        public async Task InsertProduct_EmptyRequest_ReturnsBadRequest()
        {
            // Arrange
            var content = new StringContent("", Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/v1/Product", content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [Test]
        public async Task InsertProduct_MissingName_ReturnsBadRequest()
        {
            // Arrange
            var request = new ProductInsertRequest
            {
                GlobalId = Guid.Parse("025d4482-79e2-4ad1-bbee-94e267572418"),
                ProductImageUrl = "[baseURL]/product/025d4482-79e2-4ad1-bbee-94e267572418.png",
                AvailableQuantity = 15000,
                LastUpdate = "634792557112051692",
                Price = 15.23f
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/v1/Product", content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [Test]
        public async Task InsertProduct_DuplicateName_ReturnsBadRequest()
        {
            // Arrange
            var product = _testProducts[2];

            var request = new ProductInsertRequest
            {
                GlobalId = Guid.NewGuid(),
                Name = "Test Product 2", // Duplicate name
                ProductImageUrl = "[baseURL]/product/new-product.png",
                AvailableQuantity = 500,
                LastUpdate = "634792557112051800",
                Price = 9.99f
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/v1/Product", content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [Test]
        public async Task InsertProduct_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            var request = new ProductInsertRequest
            {
                GlobalId = Guid.Parse("025d4482-79e2-4ad1-bbee-94e267572418"),
                Name = "T-Shirt Levi9",
                ProductImageUrl = "[baseURL]/product/025d4482-79e2-4ad1-bbee-94e267572418.png",
                AvailableQuantity = 15000,
                LastUpdate = "634792557112051692",
                Price = 15.23f
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = null;

            // Act
            var response = await _client.PostAsync("/v1/Product", content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        #endregion
        #region UpdateProduct
        [Test]
        public async Task UpdateProduct_ValidRequest_ReturnsOk()
        {
            // Arrange
            var updateRequest = new ProductUpdateRequest
            {
                GlobalId = Guid.Parse(_testProducts[2].GlobalId.ToString()),
                Name = "Updated Test Product 2",
                ProductImageUrl = "[baseURL]/product/updated-product.png",
                AvailableQuantity = 15000,
                LastUpdate = "133277539861042362",
                Price = 18.99f
            };

            var updateContent = new StringContent(JsonConvert.SerializeObject(updateRequest), Encoding.UTF8, "application/json");

            // Act
            var updateResponse = await _client.PutAsync("/v1/Product", updateContent);

            // Assert
            updateResponse.EnsureSuccessStatusCode();
            Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var updateResponseContent = await updateResponse.Content.ReadAsStringAsync();
            var productUpdateResponse = JsonConvert.DeserializeObject<ProductUpdateResponse>(updateResponseContent);
            Assert.That(productUpdateResponse.Name, Is.EqualTo(updateRequest.Name));
        }
        [Test]
        public async Task UpdateProduct_EmptyRequest_ReturnsBadRequest()
        {
            // Arrange
            var content = new StringContent("", Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/v1/Product", content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [Test]
        public async Task UpdateProduct_MissingGlobalId_ReturnsBadRequest()
        {
            // Arrange

            var request = new ProductUpdateRequest
            {
                Name = "Test Product 3",
                ProductImageUrl = "[baseURL]/product/updatedproduct3.png",
                AvailableQuantity = 15000,
                LastUpdate = "133277539861042363",
                Price = 18.99f
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/v1/Product", content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [Test]
        public async Task UpdateProduct_ProductNotFound_ReturnsNotFound()
        {
            // Arrange
            var request = new ProductUpdateRequest
            {
                GlobalId = Guid.NewGuid(),
                Name = "Updated T-Shirt Levi9",
                ProductImageUrl = "[baseURL]/product/025d4482-79e2-4ad1-bbee-94e267572418.png",
                AvailableQuantity = 15000,
                LastUpdate = "634792557112051692",
                Price = 18.99f
            };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/v1/Product", content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        [Test]
        public async Task UpdateProduct_InvalidName_ReturnsBadRequest()
        {
            // Arrange
            var request = new ProductUpdateRequest
            {
                GlobalId = Guid.Parse(_testProducts[5].GlobalId.ToString()),
                Name = "", // Invalid: empty name
                ProductImageUrl = "[baseURL]/product/updateproduct5.png",
                AvailableQuantity = 15000,
                LastUpdate = "133277539861042365",
                Price = 18.99f
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/v1/Product", content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [Test]
        public async Task UpdateProduct_UnauthorizedUser_ReturnsUnauthorized()
        {
            // Arrange
            var request = new ProductUpdateRequest
            {
                GlobalId = Guid.Parse(_testProducts[7].GlobalId.ToString()),
                Name = "Updated Test Product 7",
                ProductImageUrl = "[baseURL]/product/updatedproduct7.png",
                AvailableQuantity = 15000,
                LastUpdate = "133277539861042367",
                Price = 18.99f
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Authorization = null;

            // Act
            var response = await _client.PutAsync("/v1/Product", content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        #endregion
    }
}
