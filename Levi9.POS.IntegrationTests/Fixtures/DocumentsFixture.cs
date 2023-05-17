using Levi9.POS.Domain.Helpers;
using Levi9.POS.Domain.Models;
using Levi9.POS.WebApi.Request.DocumentRequest;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Levi9.POS.IntegrationTests.Fixtures
{
    public static class DocumentsFixture
    {

        public static CreateDocumentRequest GetDataForValidCreateDocument()
        {
            return new CreateDocumentRequest
            {
                ClientId = 1,
                DocumentType = "INVOICE",
                Items = new List<CreateDocumentItemRequest>
                {
                    new CreateDocumentItemRequest
                    {
                        Name = "Test Product 1",
                        ProductId = 1,
                        Price = 10.5f,
                        Currency = "USD",
                        Quantity = 100
                    },
                    new CreateDocumentItemRequest
                    {
                        Name = "Test Product 2",
                        ProductId = 2,
                        Price = 15.25f,
                        Currency = "EUR",
                        Quantity = 5
                    }
                }
            };
        }
        public static CreateDocumentRequest GetDataForInvalidClientIdCreateDocument()
        {
            return new CreateDocumentRequest
            {
                ClientId = 999999,
                DocumentType = "INVOICE",
                Items = new List<CreateDocumentItemRequest>
                {
                    new CreateDocumentItemRequest
                    {
                        Name = "Test Product 1",
                        ProductId = 1,
                        Price = 10.5f,
                        Currency = "USD",
                        Quantity = 100
                    },
                    new CreateDocumentItemRequest
                    {
                        Name = "Test Product 2",
                        ProductId = 2,
                        Price = 15.25f,
                        Currency = "EUR",
                        Quantity = 5
                    }
                }
            };
        }
        public static CreateDocumentRequest GetDataForInvalidProductIdCreateDocument()
        {
            return new CreateDocumentRequest
            {
                ClientId = 1,
                DocumentType = "INVOICE",
                Items = new List<CreateDocumentItemRequest>
                {
                    new CreateDocumentItemRequest
                    {
                        Name = "Test Product 1",
                        ProductId = 1,
                        Price = 10.5f,
                        Currency = "USD",
                        Quantity = 100
                    },
                    new CreateDocumentItemRequest
                    {
                        Name = "Test Product 2",
                        ProductId = 99999,
                        Price = 15.25f,
                        Currency = "EUR",
                        Quantity = 5
                    }
                }
            };
        }
        public static CreateDocumentRequest GetDataForInvalidDocumentInputCreateDocument()
        {
            return new CreateDocumentRequest
            {
                ClientId = 1,
                DocumentType = "TYPE",
                Items = new List<CreateDocumentItemRequest>
                {
                    new CreateDocumentItemRequest
                    {
                        Name = "Test Product 1",
                        ProductId = 1,
                        Price = -2.4f,
                        Currency = "KKD",
                        Quantity = -2
                    },
                    new CreateDocumentItemRequest
                    {
                        Name = "Test Product 2",
                        ProductId = 2,
                        Price = -15.25f,
                        Currency = "FAR",
                        Quantity = 0
                    }
                }
            };
        }
        public static Document RegisterDocument()
        {
            return new Document()
            {
                Id = 1,
                GlobalId = Guid.NewGuid(),
                LastUpdate = "133277539861042858",
                ClientId = 1,
                DocumentType = "INVOICE",
                CreationDay = "133277539861042858",
                ProductDocuments = new List<ProductDocument>()
                {
                    new ProductDocument()
                    {
                        ProductId = 1,
                        DocumentId = 1,
                        Currency = "RSD",
                        Price = 1200f,
                        Quantity = 20
                    }
                }
            };
        }
        public static Client RegisterClient()
        {
            return new Client
            {
                Id = 1,
                GlobalId = Guid.NewGuid(),
                Address = "address",
                Email = "example@gmail.com",
                Name = "name",
                LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(),
                Salt= "Salt123",
                Password = AuthenticationHelper.HashPassword("password", "Salt123"),
                Phone = "1234567890"
            };
        }
        public static List<Product> RegisterProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Id = 1,
                    GlobalId = Guid.NewGuid(),
                    Name = "Test Product 1",
                    ProductImageUrl = "https://example.com/product1.jpg",
                    AvailableQuantity = 10,
                    LastUpdate = "133277539861042364",
                    Price = 9.99f
                },
                new Product
                {
                    Id = 2,
                    GlobalId = Guid.NewGuid(),
                    Name = "Test Product 2",
                    ProductImageUrl = "https://example.com/product1.jpg",
                    AvailableQuantity = 10,
                    LastUpdate = "133277539861042364",
                    Price = 9.99f
                }
            };
        }
    }
}
