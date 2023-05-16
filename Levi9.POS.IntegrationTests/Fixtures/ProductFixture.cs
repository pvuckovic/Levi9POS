using Levi9.POS.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Levi9.POS.IntegrationTests.Fixtures
{
    public static class ProductFixture
    {
        public  static List<Product> CreateTestProducts()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    GlobalId = Guid.NewGuid(),
                    Name = "Test Product 1",
                    ProductImageUrl = "https://test.com/product1.jpg",
                    AvailableQuantity = 10,
                    LastUpdate = "133277539861042361",
                    Price = 99.99f
                },
                new Product
                {
                    Id = 2,
                    GlobalId = Guid.NewGuid(),
                    Name = "Test Product 2",
                    ProductImageUrl = "https://test.com/product2.jpg",
                    AvailableQuantity = 20,
                    LastUpdate = "133277539861042362",
                    Price = 9.65f
                },
                new Product
                {
                    Id = 3,
                    GlobalId = Guid.NewGuid(),
                    Name = "Test Product 3",
                    ProductImageUrl = "https://test.com/product3.jpg",
                    AvailableQuantity = 1112,
                    LastUpdate = "133277539861042363",
                    Price = 17.15f
                },
                new Product
                {
                    Id = 4,
                    GlobalId = Guid.NewGuid(),
                    Name = "Test Product 4",
                    ProductImageUrl = "https://test.com/product4.jpg",
                    AvailableQuantity = 14,
                    LastUpdate = "133277539861042364",
                    Price = 48.66f
                },
                new Product
                {
                    Id = 5,
                    GlobalId = Guid.NewGuid(),
                    Name = "Test Product 5",
                    ProductImageUrl = "https://test.com/product5.jpg",
                    AvailableQuantity = 365,
                    LastUpdate = "133277539861042365",
                    Price = 36.85f
                },
                new Product
                {
                    Id = 6,
                    GlobalId = Guid.NewGuid(),
                    Name = "Test Product 6",
                    ProductImageUrl = "https://test.com/product6.jpg",
                    AvailableQuantity = 758,
                    LastUpdate = "133277539861042366",
                    Price = 19.65f
                },
                new Product
                {
                    Id = 7,
                    GlobalId = Guid.NewGuid(),
                    Name = "Test Product 7",
                    ProductImageUrl = "https://test.com/product7.jpg",
                    AvailableQuantity = 1999,
                    LastUpdate = "133277539861042367",
                    Price = 10f
                }, 
                new Product
                {
                    Id = 8,
                    GlobalId = Guid.NewGuid(),
                    Name = "Test Product 8",
                    ProductImageUrl = "https://test.com/product8.jpg",
                    AvailableQuantity = 100,
                    LastUpdate = "133277539861042368",
                    Price = 48.5f
                },      
                new Product
                {
                    Id = 9,
                    GlobalId = Guid.NewGuid(),
                    Name = "Test Product 9",
                    ProductImageUrl = "https://test.com/product9.jpg",
                    AvailableQuantity = 190,
                    LastUpdate = "133277539861042369",
                    Price = 65.32f
                }, 
                new Product
                {
                    Id = 10,
                    GlobalId = Guid.NewGuid(),
                    Name = "Test Product 10",
                    ProductImageUrl = "https://test.com/product10.jpg",
                    AvailableQuantity = 13,
                    LastUpdate = "133277539861042370",
                    Price = 15.18f
                }, 
                new Product
                {
                    Id = 11,
                    GlobalId = Guid.NewGuid(),
                    Name = "Test Product 11",
                    ProductImageUrl = "https://test.com/product11.jpg",
                    AvailableQuantity = 150,
                    LastUpdate = "133277539861042371",
                    Price = 299.99f
                }
            };

            return products;
        }
        public static string GenerateJwt()
        {
            var securityKey = Encoding.UTF8.GetBytes("some-signing-key-here");
            var symetricKey = new SymmetricSecurityKey(securityKey);
            var signingCredentials = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                                            issuer: "http://localhost:5067",
                                            audience: "http://localhost:5067",
                                            expires: DateTime.Now.Add(new TimeSpan(86400)),
                                            signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
