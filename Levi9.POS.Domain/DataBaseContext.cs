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

            modelBuilder.Entity<Client>().HasData(
                new Client { Id = 1, GlobalId = new Guid("10bc28f5-7042-4736-97ad-1cb3dce98b1c"), Name = "Marko", Email = "example@gmail.com", Phone = "+387 65 132 527", Address = "1.maja, Derventa", LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), Password ="password", Salt="Salt" },
                new Client { Id = 2, GlobalId = new Guid("aa7ac410-5b2a-497e-8106-266c09f705ae"), Name = "Aleksa", Email = "aleksa@gmail.com", Phone = "+387 64 862 476", Address = "Koste Racina 24, Novi Sad", LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), Password = "password123", Salt="Salt123" },
                new Client { Id = 3, GlobalId = new Guid("5f8ac59b-1604-48fe-bcd3-7d8dbf70db08"), Name = "Milos", Email = "milos@gmail.com", Phone = "+387 65 912 127", Address = "Strumicka 13, Novi Sad", LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), Password = "password123", Salt = "Salt123" }
            );
        }
    }
}
