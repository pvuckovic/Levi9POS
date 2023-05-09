using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.Models;
using Levi9.POS.Domain.Services;
using Levi9.POS.WebApi.Mapper;
using Moq;
using NUnit.Framework;

namespace Levi9.POS.UnitTests.Services
{
    [TestFixture]
    public class DocumentServiceTests
    {
        private Mock<IDocumentRepository> _mockRepo;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IDocumentRepository>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
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
                DocumetType = "INVOICE",
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

            _mockRepo.Setup(repo => repo.GetDocumentById(1))
                .ReturnsAsync(document);
            var service = new DocumentService(_mockRepo.Object, _mapper);

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
            _mockRepo.Setup(repo => repo.GetDocumentById(1))
                .ReturnsAsync((Document)null);
            var service = new DocumentService(_mockRepo.Object, _mapper);

            // Act
            var result = await service.GetDocumentById(1);

            // Assert
            Assert.IsNull(result);
        }
    }
}
