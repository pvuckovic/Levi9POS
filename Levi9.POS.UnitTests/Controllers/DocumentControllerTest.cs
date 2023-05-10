using AutoMapper;
using Levi9.POS.Domain.Common.IDocument;
using Levi9.POS.Domain.DTOs.DocumentDTOs;
using Levi9.POS.Domain.Models.Enum;
using Levi9.POS.UnitTests.Fixtures;
using Levi9.POS.WebApi.Controllers;
using Levi9.POS.WebApi.Mapper;
using Levi9.POS.WebApi.Request;
using Levi9.POS.WebApi.Response.DocumentResponse;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;

namespace Levi9.POS.UnitTests.Controllers
{
    [TestFixture]
    public class DocumentControllerTest
    {
        private Mock<IDocumentService> _documentServiceMock;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _documentServiceMock = new Mock<IDocumentService>();
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
            var controller = new DocumentController(_documentServiceMock.Object, _mapper);

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
            var controller = new DocumentController(_documentServiceMock.Object, _mapper);

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
            var controller = new DocumentController(_documentServiceMock.Object, _mapper);

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

            var controller = new DocumentController(_documentServiceMock.Object, _mapper);

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

            var controller = new DocumentController(_documentServiceMock.Object, _mapper);

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

            var controller = new DocumentController(_documentServiceMock.Object, _mapper);

            // Act
            var result = await controller.CreateDocument(documentRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Product does not exist!", badRequestResult.Value);
        }
    }
}
