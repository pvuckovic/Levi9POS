using Levi9.POS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Levi9.POS.Data.Migrations
{
    public class DataBaseContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<ProductDocument> ProductDocuments { get; set; }
        public DataBaseContext(DbContextOptions<DataBaseContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductDocument>()
                .HasKey(e => new { e.ProductId, e.DocumentId });

            modelBuilder.Entity<ProductDocument>()
                .HasOne(e => e.Product)
                .WithMany(e=>e.ProductDocuments)
            .HasForeignKey(e => e.ProductId);
            modelBuilder.Entity<ProductDocument>()
                .HasOne(e => e.Document)
                .WithMany(e=>e.ProductDocuments)
                .HasForeignKey(e => e.DocumentId);

            modelBuilder.Entity<Client>()
                .HasIndex(e => e.Email).IsUnique();
            modelBuilder.Entity<Product>()
                .HasIndex(e => e.Name).IsUnique();
        }
    }
}

