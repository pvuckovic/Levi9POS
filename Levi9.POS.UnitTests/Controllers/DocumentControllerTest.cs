using AutoMapper;
using IdentityModel.OidcClient;
using Levi9.POS.Domain.Common.IDocument;
using Levi9.POS.Domain.DTOs.DocumentDTOs;
using Levi9.POS.Domain.Models.Enum;
using Levi9.POS.UnitTests.Fixtures;
using Levi9.POS.WebApi.Controllers;
using Levi9.POS.WebApi.Mapper;
using Levi9.POS.WebApi.Request;
using Levi9.POS.WebApi.Response;
using Levi9.POS.WebApi.Response.DocumentResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;

namespace Levi9.POS.UnitTests.Controllers
{
    [TestFixture]
    public class DocumentControllerTest
    {
        private Mock<IDocumentService> _documentServiceMock;
        private Mock<ILogger<DocumentController>> _loggerMock;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _documentServiceMock = new Mock<IDocumentService>();
            _loggerMock = new Mock<ILogger<DocumentController>>();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DocumentMappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public async Task GetDocumentById_ReturnsOkResult_WhenDocumentExists()
        {
            // Arrange
            int documentId = 1;
            var document = DocumentsFixture.GetDataForGetDocumentByIdDocumentController();
            _documentServiceMock.Setup(s => s.GetDocumentById(documentId)).ReturnsAsync(document);
            var controller = new DocumentController(_documentServiceMock.Object, _loggerMock.Object, _mapper);

            // Act
            var result = await controller.GetDocumentById(documentId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as GetByIdDocumentResponse;
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Items);
            Assert.AreEqual(documentId, response.Id);
        }

        [Test]
        public async Task GetDocumentById_ReturnsNotFoundResult_WhenDocumentDoesNotExist()
        {
            // Arrange
            int documentId = 1;
            _documentServiceMock.Setup(s => s.GetDocumentById(documentId)).ReturnsAsync((DocumentDTO)null);
            var controller = new DocumentController(_documentServiceMock.Object, _loggerMock.Object, _mapper);

            // Act
            var result = await controller.GetDocumentById(documentId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual("There is no document with the desired ID.", notFoundResult.Value);
        }

        [Test]
        public async Task GetDocumentById_ReturnsBadRequestResult_WhenDocumentIdIsInvalid()
        {
            // Arrange
            int documentId = -1;
            var controller = new DocumentController(_documentServiceMock.Object, _loggerMock.Object, _mapper);

            // Act
            var result = await controller.GetDocumentById(documentId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("The ID must be a positive number.", badRequestResult.Value);
        }

        [Test]
        public async Task CreateDocument_WhenDocumentIsValid_ReturnsOk()
        {
            // Arrange
            var documentRequest = DocumentsFixture.GetDataForCreateDocumentController();

            _documentServiceMock
                .Setup(x => x.CreateDocument(It.IsAny<CreateDocumentDTO>()))
                .ReturnsAsync(CreateDocumentResult.Success);

            var controller = new DocumentController(_documentServiceMock.Object, _loggerMock.Object, _mapper);

            // Act
            var result = await controller.CreateDocument(documentRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Document created successfully", okResult.Value);
        }

        [Test]
        public async Task CreateDocument_WhenClientNotFound_ReturnsBadRequest()
        {
            // Arrange
            var documentRequest = DocumentsFixture.GetDataForCreateDocumentController();

            _documentServiceMock
                .Setup(x => x.CreateDocument(It.IsAny<CreateDocumentDTO>()))
                .ReturnsAsync(CreateDocumentResult.ClientNotFound);

            var controller = new DocumentController(_documentServiceMock.Object, _loggerMock.Object, _mapper);

            // Act
            var result = await controller.CreateDocument(documentRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Client does not exist!", badRequestResult.Value);
        }

        [Test]
        public async Task CreateDocument_WhenProductNotFound_ReturnsBadRequest()
        {
            // Arrange
            var documentRequest = DocumentsFixture.GetDataForCreateDocumentController();

            _documentServiceMock
                .Setup(x => x.CreateDocument(It.IsAny<CreateDocumentDTO>()))
                .ReturnsAsync(CreateDocumentResult.ProductNotFound);

            var controller = new DocumentController(_documentServiceMock.Object, _loggerMock.Object, _mapper);

            // Act
            var result = await controller.CreateDocument(documentRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Product does not exist!", badRequestResult.Value);
        }
        [Test]
        public async Task GetAllDocuments_ValidRequest_ReturnsOkWithMappedDocuments()
        {
            string lastUpdate = "123456789987654321";

            var documents = new List<DocumentSyncDto>
            {
                new DocumentSyncDto
                {
                    GlobalId = Guid.NewGuid(),
                    ClientId = Guid.NewGuid(),
                    DocumentType = "INVOICE",
                    Items = new List<DocumentItemSyncDto>
                    {
                        new DocumentItemSyncDto
                        {
                            Name = "Item 1",
                            ProductId = Guid.NewGuid(),
                            Price = 10.0f,
                            Currency = "USD",
                            Quantity = 2
                        },
                    }
                },
                new DocumentSyncDto
                {
                    GlobalId = Guid.NewGuid(),
                    ClientId = Guid.NewGuid(),
                    DocumentType = "INVOICE",
                    Items = new List<DocumentItemSyncDto>
                    {
                        new DocumentItemSyncDto
                        {
                            Name = "Item 2",
                            ProductId = Guid.NewGuid(),
                            Price = 20.0f,
                            Currency = "USD",
                            Quantity = 1
                        }
                    }
                }
            };

            var expectedResponse = documents.Select(d => new DocumentSyncResponse
            {
                GlobalId = d.GlobalId,
                ClientId = d.ClientId,
                DocumentType = d.DocumentType,
                Items = d.Items.Select(i => new DocumentItemSyncResponse
                {
                    Name = i.Name,
                    ProductId = i.ProductId,
                    Price = i.Price,
                    Currency = i.Currency,
                    Quantity = i.Quantity
                }).ToList()
            }).ToList();

            _documentServiceMock.Setup(x => x.GetDocumentsByLastUpdate(lastUpdate)).ReturnsAsync(documents);

            var controller = new DocumentController(_documentServiceMock.Object, _loggerMock.Object, _mapper);

            var result = await controller.GetAllDocuments(lastUpdate);
            var okResult = result as OkObjectResult;
            var responseList = okResult.Value as IEnumerable<DocumentSyncResponse>;
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<OkObjectResult>());
                Assert.That(okResult.Value, Is.InstanceOf<IEnumerable<DocumentSyncResponse>>());
                Assert.That(responseList.Count, Is.EqualTo(expectedResponse.Count));
            });
        }

        [Test]
        public async Task GetAllProducts_ReturnsOkWithEmptyList_WhenServiceReturnsEmptyList()
        {
            var lastUpdate = "123456789987654321";
            var emptyList = Enumerable.Empty<DocumentSyncDto>();
            _documentServiceMock.Setup(x => x.GetDocumentsByLastUpdate(lastUpdate)).ReturnsAsync(emptyList);
            var controller = new DocumentController(_documentServiceMock.Object, _loggerMock.Object, _mapper);

            var result = await controller.GetAllDocuments(lastUpdate);

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var responseList = okResult.Value as IEnumerable<DocumentSyncDto>;
            CollectionAssert.AreEqual(responseList, emptyList);
        }
    }
}