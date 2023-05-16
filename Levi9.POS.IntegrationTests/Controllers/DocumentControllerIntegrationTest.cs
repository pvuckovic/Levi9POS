using Levi9.POS.Domain;
using Levi9.POS.Domain.Models;
using Levi9.POS.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Levi9.POS.IntegrationTests.Controllers
{
    [TestFixture]
    public class DocumentContollerIntegrationTest
    {

        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private DataBaseContext _dbContext;

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
                    _dbContext.Database.EnsureCreated();
                });
            });

            // Create an instance of HttpClient for making requests
            _client = _factory.CreateClient();
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

        [Test]
        public async Task GetDocumentById_ValidId_ReturnsOk()
        {
            // Arrange
            // Act
            string token = DocumentsFixture.GenerateJwt();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync("/v1/document/1");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Document>(content);
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public async Task GetDocumentById_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            // Act
            string token = DocumentsFixture.GenerateJwt();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync("/v1/document/0");
            var result = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("The ID must be a positive number.", result);
        }

        [Test]
        public async Task GetDocumentById_NoDocumentFound_ReturnsNotFound()
        {
            // Arrange
            // Act
            string token = DocumentsFixture.GenerateJwt();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync("/v1/document/10000");
            var result = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.AreEqual("There is no document with the desired ID.", result);
        }


        [Test]
        public async Task GetDocumentById_InvalidToken_ReturnsUnauthorized()
        {
            // Arrange
            // Act
            string token = "Invalid.token.value";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync("/v1/document/1");

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Test]
        public async Task CreateDocument_ValidDocument_ReturnsOk()
        {
            // Arrange
            // Act
            string token = DocumentsFixture.GenerateJwt();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var document = DocumentsFixture.GetDataForValidCreateDocument();

            var jsonRequest = JsonConvert.SerializeObject(document);
            var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/v1/document", httpContent);
            var result = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Document created successfully", result);
        }

        [Test]
        public async Task CreateDocument_InvalidClientId_ReturnsBadRequest()
        {
            // Arrange
            // Act
            string token = DocumentsFixture.GenerateJwt();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var document = DocumentsFixture.GetDataForInvalidClientIdCreateDocument();

            var jsonRequest = JsonConvert.SerializeObject(document);
            var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/v1/document", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("Client does not exist!", result);
        }

        [Test]
        public async Task CreateDocument_InvalidProductId_ReturnsBadRequest()
        {
            // Arrange
            // Act
            string token = DocumentsFixture.GenerateJwt();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var document = DocumentsFixture.GetDataForInvalidProductIdCreateDocument();

            var jsonRequest = JsonConvert.SerializeObject(document);
            var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/v1/document", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("Product does not exist!", result);
        }

        [Test]
        public async Task CreateDocument_InvalidDocumentInput_ReturnsBadRequest()
        {
            // Arrange
            // Act
            string token = DocumentsFixture.GenerateJwt();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var document = DocumentsFixture.GetDataForInvalidDocumentInputCreateDocument();

            var jsonRequest = JsonConvert.SerializeObject(document);
            var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/v1/document", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task CreateDocument_InvalidToken_ReturnsUnauthorized()
        {
            // Arrange
            // Act
            string token = "Invalid.token.value";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var document = DocumentsFixture.GetDataForValidCreateDocument();

            var jsonRequest = JsonConvert.SerializeObject(document);
            var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/v1/document", httpContent);
            var result = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }


    }
}
