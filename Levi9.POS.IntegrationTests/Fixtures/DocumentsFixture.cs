using Levi9.POS.Domain.Models;
using Levi9.POS.WebApi.Request.DocumentRequest;
using Microsoft.IdentityModel.Tokens;
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
                        Name = "Novis T-Shirt",
                        ProductId = 2,
                        Price = 10.5f,
                        Currency = "USD",
                        Quantity = 100
                    },
                    new CreateDocumentItemRequest
                    {
                        Name = "Vega IT T-Shirt",
                        ProductId = 3,
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
                        Name = "Novis T-Shirt",
                        ProductId = 2,
                        Price = 10.5f,
                        Currency = "USD",
                        Quantity = 100
                    },
                    new CreateDocumentItemRequest
                    {
                        Name = "Vega IT T-Shirt",
                        ProductId = 3,
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
                        Name = "Novis T-Shirt",
                        ProductId = 2,
                        Price = 10.5f,
                        Currency = "USD",
                        Quantity = 100
                    },
                    new CreateDocumentItemRequest
                    {
                        Name = "Vega IT T-Shirt",
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
                        Name = "Novis T-Shirt",
                        ProductId = 2,
                        Price = -2.4f,
                        Currency = "KKD",
                        Quantity = -2
                    },
                    new CreateDocumentItemRequest
                    {
                        Name = "Vega IT T-Shirt",
                        ProductId = 3,
                        Price = -15.25f,
                        Currency = "FAR",
                        Quantity = 0
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
