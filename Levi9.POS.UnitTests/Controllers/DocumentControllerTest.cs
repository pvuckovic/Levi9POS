using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.WebApi.Controllers;
using Levi9.POS.WebApi.Mapper;
using Levi9.POS.WebApi.Response;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

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
                mc.AddProfile(new MappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public async Task GetDocumentById_ReturnsOkResult_WhenDocumentExists()
        {
            // Arrange
            int documentId = 1;
            var items = new DocumentItemDTO
            {
                Name = "Levi 9 T-Shirt",
                Price = 1200,
                Quantity = 20,
                LastUpdate = "133277539861042858"
            };
            var document = new DocumentDTO { 
                Id = documentId,
                GlobalId = Guid.NewGuid(),
                LastUpdate = "133277539861042858",
                ClientId = 1,
                DocumetType = "INVOICE",
                CreationDay = "133277539861042858"
            };
            _documentServiceMock.Setup(s => s.GetDocumentById(documentId)).ReturnsAsync(document);

            var controller = new DocumentController(_documentServiceMock.Object, _mapper);

            // Act
            var result = await controller.GetDocumentById(documentId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as DocumentResponse;
            Assert.IsNotNull(response);
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
    }
}
