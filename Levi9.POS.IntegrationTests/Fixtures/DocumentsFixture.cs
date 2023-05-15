using Levi9.POS.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Levi9.POS.IntegrationTests.Fixtures
{
    public static class DocumentsFixture
    {
        public static Document GetDataForDocumentRepository()
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
                        Product = new Product()
                        {
                            Id = 1,
                            Name = "Levi 9 T-Shirt",
                            GlobalId = Guid.NewGuid(),
                            ProductImageUrl = "baseURL//nekiurl1.png",
                            AvailableQuantity = 30,
                            Price = 60,
                            LastUpdate = DateTime.Now.AddDays(-1).ToFileTimeUtc().ToString()
                        },
                        Price = 1200f,
                        Quantity = 20
                    }
                }
            };
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
