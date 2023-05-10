using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.Common.IDocument;
using Levi9.POS.Domain.Common.IProduct;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Models;
using Levi9.POS.Domain.Models.Enum;
using Levi9.POS.Domain.Services;
using Levi9.POS.UnitTests.Fixtures;
using Levi9.POS.WebApi.Mapper;
using Moq;
using NUnit.Framework;

namespace Levi9.POS.UnitTests.Services
{
    [TestFixture]
    public class DocumentServiceTests
    {
        private Mock<IDocumentRepository> _documentRepositoryMock;
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<IClientRepository> _clientRepositoryMock;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _documentRepositoryMock = new Mock<IDocumentRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _clientRepositoryMock = new Mock<IClientRepository>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DocumentMappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        [Test]
        public async Task GetDocumentById_ReturnsDocument_WhenDocumentExists()
        {
            // Arrange
            var document = new Document()
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

            _documentRepositoryMock.Setup(repo => repo.GetDocumentById(1))
                .ReturnsAsync(document);
            var service = new DocumentService(_documentRepositoryMock.Object, _productRepositoryMock.Object, _clientRepositoryMock.Object, _mapper);

            // Act
            var result = await service.GetDocumentById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public async Task GetDocumentById_ReturnsNull_WhenDocumentDoesNotExist()
        {
            // Arrange
            _documentRepositoryMock.Setup(repo => repo.GetDocumentById(1))
                .ReturnsAsync((Document)null);
            var service = new DocumentService(_documentRepositoryMock.Object, _productRepositoryMock.Object, _clientRepositoryMock.Object, _mapper);

            // Act
            var result = await service.GetDocumentById(1);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task CreateDocument_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var newDocument = DocumentsFixture.GetDataForCreateDocumentService();
            _clientRepositoryMock.Setup(x => x.DoesClientExist(newDocument.ClientId)).ReturnsAsync(true);
            _productRepositoryMock.Setup(x => x.DoesProductExist(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);
            var service = new DocumentService(_documentRepositoryMock.Object, _productRepositoryMock.Object, _clientRepositoryMock.Object, _mapper);

            // Act
            var result = await service.CreateDocument(newDocument);

            // Assert
            Assert.AreEqual(CreateDocumentResult.Success, result);
        }

        [Test]
        public async Task CreateDocument_WithInvalidClientId_ReturnsClientNotFound()
        {
            // Arrange
            var newDocument = DocumentsFixture.GetDataForCreateDocumentService();
            _clientRepositoryMock.Setup(x => x.DoesClientExist(newDocument.ClientId)).ReturnsAsync(false);
            var service = new DocumentService(_documentRepositoryMock.Object, _productRepositoryMock.Object, _clientRepositoryMock.Object, _mapper);

            // Act
            var result = await service.CreateDocument(newDocument);

            // Assert
            Assert.AreEqual(CreateDocumentResult.ClientNotFound, result);
        }

        [Test]
        public async Task CreateDocument_ProductNotFound_ReturnsProductNotFoundResult()
        {
            // Arrange
            var newDocument = DocumentsFixture.GetDataForCreateDocumentService();
            _clientRepositoryMock.Setup(x => x.DoesClientExist(newDocument.ClientId))
                .ReturnsAsync(true);
            _productRepositoryMock.Setup(x => x.DoesProductExist(1, "Product1"))
                .ReturnsAsync(true);
            _productRepositoryMock.Setup(x => x.DoesProductExist(2, "Product2"))
                .ReturnsAsync(false);
            var service = new DocumentService(_documentRepositoryMock.Object, _productRepositoryMock.Object, _clientRepositoryMock.Object, _mapper);

            // Act
            var result = await service.CreateDocument(newDocument);

            // Assert
            Assert.AreEqual(CreateDocumentResult.ProductNotFound, result);
        }
    }
}
