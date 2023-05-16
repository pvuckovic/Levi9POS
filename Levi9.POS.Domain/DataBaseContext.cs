using Levi9.POS.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Levi9.POS.Domain
{
    public class DataBaseContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<ProductDocument> ProductDocuments { get; set; }
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductDocument>()
                .HasKey(e => new { e.ProductId, e.DocumentId });

            modelBuilder.Entity<ProductDocument>()
                .HasOne(e => e.Product)
                .WithMany(e => e.ProductDocuments)
            .HasForeignKey(e => e.ProductId);
            modelBuilder.Entity<ProductDocument>()
                .HasOne(e => e.Document)
                .WithMany(e => e.ProductDocuments)
                .HasForeignKey(e => e.DocumentId);

            modelBuilder.Entity<Client>()
                .HasIndex(e => e.Email).IsUnique();
            modelBuilder.Entity<Product>()
                .HasIndex(e => e.Name).IsUnique();

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, GlobalId = Guid.NewGuid(), ProductImageUrl = $"baseURL//nekiurl1.png", Name = "Levi 9 T-Shirt", AvailableQuantity = 30, LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), Price = 10 },
                new Product { Id = 2, GlobalId = Guid.NewGuid(), ProductImageUrl = $"baseURL//nekiurl2.png", Name = "Novis T-Shirt", AvailableQuantity = 10, LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), Price = 15 },
                new Product { Id = 3, GlobalId = Guid.NewGuid(), ProductImageUrl = $"baseURL//nekiurl3.png", Name = "Vega IT T-Shirt", AvailableQuantity = 20, LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), Price = 20 }
            );

            modelBuilder.Entity<Client>().HasData(
                new Client { Id = 1, GlobalId = new Guid("10bc28f5-7042-4736-97ad-1cb3dce98b1c"), Name = "Marko", Email = "example@gmail.com", Phone = "+387 65 132 527", Address = "1.maja, Derventa", LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), Password ="password", Salt="Salt" },
                new Client { Id = 2, GlobalId = new Guid("aa7ac410-5b2a-497e-8106-266c09f705ae"), Name = "Aleksa", Email = "aleksa@gmail.com", Phone = "+387 64 862 476", Address = "Koste Racina 24, Novi Sad", LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), Password = "password123", Salt="Salt123" },
                new Client { Id = 3, GlobalId = new Guid("5f8ac59b-1604-48fe-bcd3-7d8dbf70db08"), Name = "Milos", Email = "milos@gmail.com", Phone = "+387 65 912 127", Address = "Strumicka 13, Novi Sad", LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), Password = "password123", Salt = "Salt123" }
            );

            modelBuilder.Entity<Document>().HasData(
                new Document { Id = 1, GlobalId = Guid.NewGuid(), ClientId = 1, CreationDay = DateTime.Now.ToFileTimeUtc().ToString(), LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), DocumentType = "INVOICE" },
                new Document { Id = 2, GlobalId = Guid.NewGuid(), ClientId = 2, CreationDay = DateTime.Now.ToFileTimeUtc().ToString(), LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), DocumentType = "INVOICE" },
                new Document { Id = 3, GlobalId = Guid.NewGuid(), ClientId = 3, CreationDay = DateTime.Now.ToFileTimeUtc().ToString(), LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), DocumentType = "INVOICE" }
            );

            modelBuilder.Entity<ProductDocument>().HasData(
                new ProductDocument { ProductId = 1, DocumentId = 1, Currency = "RSD", Price = 1200, Quantity = 20 },
                new ProductDocument { ProductId = 2, DocumentId = 2, Currency = "EUR", Price = 10, Quantity = 10 },
                new ProductDocument { ProductId = 3, DocumentId = 3, Currency = "USD", Price = 15, Quantity = 15 }
            );
        }
    }
}
