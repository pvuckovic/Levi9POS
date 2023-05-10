using Levi9.POS.Domain.DTOs.DocumentDTOs;
using Levi9.POS.Domain.Models;
using Levi9.POS.WebApi.Request.DocumentRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Levi9.POS.UnitTests.Fixtures
{
    public static class DocumentsFixture
    {
        public static DocumentDTO GetDataForGetDocumentByIdDocumentController()
        {
            return new DocumentDTO
            {
                Id = 1,
                GlobalId = Guid.NewGuid(),
                LastUpdate = "133277539861042858",
                ClientId = 1,
                DocumetType = "INVOICE",
                CreationDay = "133277539861042858",
                DocumentItems = new List<DocumentItemDTO>
                {
                    new DocumentItemDTO
                    {
                        Name = "Levi 9 T-Shirt",
                        Price = 1200,
                        Quantity = 20,
                        LastUpdate = "133277539861042858"
                    }
                }
            };
        }
        public static CreateDocumentRequest GetDataForCreateDocumentController()
        {
            return new CreateDocumentRequest
            {
                ClientId = 1,
                DocumentType = "INVOICE",
                Items = new List<CreateDocumentItemRequest>
            {
                new CreateDocumentItemRequest
                {
                    Name = "Item 1",
                    ProductId = 1,
                    Price = 10.5f,
                    Currency = "USD",
                    Quantity = 2
                },
                new CreateDocumentItemRequest
                {
                    Name = "Item 2",
                    ProductId = 2,
                    Price = 15.25f,
                    Currency = "EUR",
                    Quantity = 1
                }
            }
            };
        }
        public static CreateDocumentDTO GetDataForCreateDocumentService()
        {
            return new CreateDocumentDTO
            {
                ClientId = 1,
                DocumentType = "INVOICE",
                DocumentItems = new List<CreateDocumentItemDTO>
                {
                    new CreateDocumentItemDTO 
                    { 
                        ProductId = 1, 
                        Name = "Product1", 
                        Price = 10, 
                        Quantity = 2, 
                        Currency = "USD" 
                    },
                    new CreateDocumentItemDTO 
                    { 
                        ProductId = 2, 
                        Name = "Product2", 
                        Price = 20, 
                        Quantity = 3, 
                        Currency = "USD" }
                }
            };
        }
    }
}
