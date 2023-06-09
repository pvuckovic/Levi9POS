﻿using Levi9.POS.Domain;
using Levi9.POS.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Levi9.POS.IntegrationTests
{
    public class CustomWebAppFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        private static DbContextOptions<DataBaseContext> _dataBaseContextOptions = new DbContextOptionsBuilder<DataBaseContext>()
            .UseInMemoryDatabase("TestDB")
            .Options;
        DataBaseContext _dataBaseContext;
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
                    options.UseInMemoryDatabase("TestDB");
                });

                // Build the service provider to resolve and initialize the database context
                var serviceProvider = services.BuildServiceProvider();

                _dataBaseContext = serviceProvider.GetRequiredService<DataBaseContext>();
                _dataBaseContext.Database.EnsureDeleted();

                _dataBaseContext = new DataBaseContext(_dataBaseContextOptions);
                SeedDataBase();
            });
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _dataBaseContext.Database.EnsureDeleted();
        }

        private void SeedDataBase()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, GlobalId = Guid.NewGuid(), ProductImageUrl = $"baseURL//nekiurl1.png", Name = "Levi 9 T-Shirt", AvailableQuantity = 30, LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), Price = 10 },
                new Product { Id = 2, GlobalId = Guid.NewGuid(), ProductImageUrl = $"baseURL//nekiurl2.png", Name = "Novis T-Shirt", AvailableQuantity = 10, LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), Price = 15 },
                new Product { Id = 3, GlobalId = Guid.NewGuid(), ProductImageUrl = $"baseURL//nekiurl3.png", Name = "Vega IT T-Shirt", AvailableQuantity = 20, LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), Price = 20 }

            };
            _dataBaseContext.Products.AddRange(products);

            var clients = new List<Client>
            {
                new Client { Id = 1, GlobalId = new Guid("10bc28f5-7042-4736-97ad-1cb3dce98b1c"), Name = "Marko", Email = "example@gmail.com", Phone = "+387 65 132 527", Address = "1.maja, Derventa", LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), Password ="password", Salt="Salt" },
                new Client { Id = 2, GlobalId = new Guid("aa7ac410-5b2a-497e-8106-266c09f705ae"), Name = "Aleksa", Email = "aleksa@gmail.com", Phone = "+387 64 862 476", Address = "Koste Racina 24, Novi Sad", LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), Password = "password123", Salt="Salt123" },
                new Client { Id = 3, GlobalId = new Guid("5f8ac59b-1604-48fe-bcd3-7d8dbf70db08"), Name = "Milos", Email = "milos@gmail.com", Phone = "+387 65 912 127", Address = "Strumicka 13, Novi Sad", LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), Password = "password123", Salt = "Salt123" }
            };
            _dataBaseContext.Clients.AddRange(clients);

            var documents = new List<Document>
            {
                new Document { Id = 1, GlobalId = Guid.NewGuid(), ClientId = 1, CreationDay = DateTime.Now.ToFileTimeUtc().ToString(), LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), DocumentType = "INVOICE" },
                new Document { Id = 2, GlobalId = Guid.NewGuid(), ClientId = 2, CreationDay = DateTime.Now.ToFileTimeUtc().ToString(), LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), DocumentType = "INVOICE" },
                new Document { Id = 3, GlobalId = Guid.NewGuid(), ClientId = 3, CreationDay = DateTime.Now.ToFileTimeUtc().ToString(), LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), DocumentType = "INVOICE" }
            };
            _dataBaseContext.Documents.AddRange(documents);

            var prodDocs = new List<ProductDocument>
            {
                new ProductDocument { ProductId = 1, DocumentId = 1, Currency = "RSD", Price = 1200, Quantity = 20 },
                new ProductDocument { ProductId = 2, DocumentId = 2, Currency = "EUR", Price = 10, Quantity = 10 },
                new ProductDocument { ProductId = 3, DocumentId = 3, Currency = "USD", Price = 15, Quantity = 15 }
            };
            _dataBaseContext.ProductDocuments.AddRange(prodDocs);
            _dataBaseContext.SaveChanges();
        }
    }
}
