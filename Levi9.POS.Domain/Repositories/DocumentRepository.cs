using Levi9.POS.Domain.Common.IDocument;
using Levi9.POS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Levi9.POS.Domain.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DataBaseContext _data;
        private readonly ILogger<DocumentRepository> _logger;
        public DocumentRepository(DataBaseContext data, ILogger<DocumentRepository> logger) 
        {
            _data = data;
            _logger = logger;
        }
        public async Task<Document> GetDocumentById(int id)
        {
            _logger.LogInformation("Entering {FunctionName} in DocumentRepository. Timestamp: {Timestamp}.", nameof(GetDocumentById), DateTime.UtcNow);            
            var result = await _data.Documents
                .Include(d => d.ProductDocuments)
                    .ThenInclude(pd => pd.Product)
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync();

            if (result == null)
            {
                _logger.LogWarning("Document not found with ID: {DocumentId} in {FunctionName} of DocumentRepository. Timestamp: {Timestamp}.", id, nameof(GetDocumentById), DateTime.UtcNow);
                return null;
            }

            _logger.LogInformation("Document retrieved successfully with ID: {DocumentId} in {FunctionName} of DocumentRepository. Timestamp: {Timestamp}.", id, nameof(GetDocumentById), DateTime.UtcNow);
            return result;
        }
        public async Task<Document> CreateDocument(Document newDocument)
        {
            _logger.LogInformation("Entering {FunctionName} in DocumentRepository. Timestamp: {Timestamp}.", nameof(CreateDocument), DateTime.UtcNow);
            var result = await _data.Documents.AddAsync(newDocument);
            await _data.SaveChangesAsync();
            _logger.LogInformation("Document created successfully in {FunctionName} of DocumentRepository. Timestamp: {Timestamp}.", nameof(CreateDocument), DateTime.UtcNow);
            return result.Entity;
        }

        public async Task<IEnumerable<Document>> GetDocumentsByLastUpdate(string lastUpdate)
        {
            _logger.LogInformation("Entering {FunctionName} in DocumentRepository. Timestamp: {Timestamp}.", nameof(GetDocumentsByLastUpdate), DateTime.UtcNow);
            return await _data.Documents
                            .Where(d => string.Compare(d.LastUpdate, lastUpdate) > 0)
                            .Include(pd => pd.Client)
                            .Include(d => d.ProductDocuments)
                            .ThenInclude(p => p.Product)
                            .ToListAsync();
        }
    }
}
