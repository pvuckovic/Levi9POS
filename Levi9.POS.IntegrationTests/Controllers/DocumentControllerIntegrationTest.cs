using Levi9.POS.Domain;
using Levi9.POS.Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http.Headers;

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
            //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "your_token_here");

            var response = await _client.GetAsync("/v1/document/1");
            

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Document>(content);
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
        }
        
    }
}
