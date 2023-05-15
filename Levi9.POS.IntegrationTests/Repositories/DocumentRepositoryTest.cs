using Levi9.POS.Domain;
using Levi9.POS.Domain.Models;
using Levi9.POS.Domain.Repositories;
using Levi9.POS.IntegrationTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Levi9.POS.IntegrationTests.Repositories
{
    [TestFixture]
    public class DocumentRepositoryTests
    {
        private DataBaseContext _dbContext;
        private DocumentRepository _repository;
        private ILogger<DocumentRepository> _logger;

        [SetUp]
        public void Setup()
        {
            // Set up an in-memory database for testing
            var options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Create a new instance of the database context
            _dbContext = new DataBaseContext(options);

            // Mock the logger
            _logger = Mock.Of<ILogger<DocumentRepository>>();

            // Create the repository using the in-memory database and mocked logger
            _repository = new DocumentRepository(_dbContext, _logger);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up the in-memory database after each test
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetDocumentById_ExistingId_ReturnsDocument()
        {
            // Arrange
            var existingDocument = DocumentsFixture.GetDataForDocumentRepository();
            _dbContext.Documents.Add(existingDocument);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetDocumentById(existingDocument.Id);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(existingDocument.Id, result.Id);
            Assert.AreEqual(existingDocument.GlobalId, result.GlobalId);
            Assert.AreEqual(existingDocument.LastUpdate, result.LastUpdate);
            Assert.AreEqual(existingDocument.ClientId, result.ClientId);
            Assert.AreEqual(existingDocument.DocumentType, result.DocumentType);
            Assert.AreEqual(existingDocument.CreationDay, result.CreationDay);
            Assert.IsNotNull(result.ProductDocuments);
        }

        [Test]
        public async Task GetDocumentById_NonExistingId_ReturnsNull()
        {
            // Arrange
            var nonExistingId = 999;

            // Act
            var result = await _repository.GetDocumentById(nonExistingId);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task CreateDocument_NewDocument_CreatesDocument()
        {
            // Arrange
            var newDocument = DocumentsFixture.GetDataForDocumentRepository();

            // Act
            var result = await _repository.CreateDocument(newDocument);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(newDocument.Id, result.Id);
            Assert.AreEqual(newDocument.GlobalId, result.GlobalId);
            Assert.AreEqual(newDocument.LastUpdate, result.LastUpdate);
            Assert.AreEqual(newDocument.ClientId, result.ClientId);
            Assert.AreEqual(newDocument.DocumentType, result.DocumentType);
            Assert.AreEqual(newDocument.CreationDay, result.CreationDay);
            Assert.AreNotEqual(0, result.Id);
            Assert.IsNotNull(result.ProductDocuments);

            // Check if the document is saved in the in-memory database
            var savedDocument = _dbContext.Documents.FirstOrDefault(d => d.Id == result.Id);
            Assert.NotNull(savedDocument);
            Assert.AreEqual(newDocument.Id, savedDocument.Id);
            Assert.AreEqual(newDocument.GlobalId, savedDocument.GlobalId);
            Assert.AreEqual(newDocument.LastUpdate, savedDocument.LastUpdate);
            Assert.AreEqual(newDocument.ClientId, savedDocument.ClientId);
            Assert.AreEqual(newDocument.DocumentType, savedDocument.DocumentType);
            Assert.AreEqual(newDocument.CreationDay, savedDocument.CreationDay);
            Assert.AreNotEqual(0, savedDocument.Id);
            Assert.IsNotNull(savedDocument.ProductDocuments);
        }
    }
}
