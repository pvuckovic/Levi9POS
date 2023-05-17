using Levi9.POS.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Levi9.POS.IntegrationTests
{
    public class CustomWebAppFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        private DataBaseContext _dataBaseContext;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
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
                _dataBaseContext = serviceProvider.GetRequiredService<DataBaseContext>();
                _dataBaseContext.Database.EnsureDeleted();
                _dataBaseContext.Database.EnsureCreated();
            });
        }
    }
}
