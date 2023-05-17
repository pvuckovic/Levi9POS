using System.Net;
using System.Net.Http.Json;
using System.Text;
using Levi9.POS.Domain;
using Levi9.POS.Domain.Models;
using Levi9.POS.IntegrationTests.Fixtures;
using Levi9.POS.WebApi.Request.ClientRequests;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Levi9.POS.IntegrationTests.Controllers
{
    [TestFixture]
    public class AuthenticationControllerIntegrationTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private DataBaseContext _dbContext;
        private List<Client> _testClient;

        [SetUp]
        public void SetUp()
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
                    if (!_dbContext.Clients.Any())
                    {
                        // Generate test products
                        _testClient = AuthenticationFixture.CreateTestClient();
                        _dbContext.Clients.AddRange(_testClient);
                        _dbContext.SaveChanges();
                    }
                });
            });

            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up the in-memory database and dispose of resources
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
            _client.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task ClientAuthentication_ValidCredentials_ReturnsOk()
        {
            // Arrange
            var request = new ClientLogin
            {
                Email = _testClient[1].Email,
                Password = "test2password"
            };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsJsonAsync("/v1/Authentication", request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var token = await response.Content.ReadAsStringAsync();
            Assert.That(string.IsNullOrEmpty(token), Is.False);
        }

        [Test]
        public async Task ClientAuthentication_InvalidEmail_ReturnsBadRequest()
        {
            // Arrange
            var request = new ClientLogin
            {
                Email = "invalid@example.com",
                Password = "test123"
            };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/v1/Authentication", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task ClientAuthentication_InvalidPassword_ReturnsBadRequest()
        {
            // Arrange
            var request = new ClientLogin
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/v1/Authentication", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
