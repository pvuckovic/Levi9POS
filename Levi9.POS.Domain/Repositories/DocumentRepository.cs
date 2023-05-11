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
            _logger.LogInformation("Entering {FunctionName} in document repository.", nameof(GetDocumentById));            
            var result = await _data.Documents
                .Include(d => d.ProductDocuments)
                    .ThenInclude(pd => pd.Product)
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync();

            if (result == null)
            {
                _logger.LogWarning("Document not found with this ID: {DocumentId} in {FunctionName} of DocumentRepository. Timestamp: {Timestamp}.", id, nameof(GetDocumentById), DateTime.UtcNow);
                return null;
            }

            _logger.LogInformation("Document retrieved successfully with this ID: {DocumentId} in {FunctionName} of DocumentRepository. Timestamp: {Timestamp}.", id, nameof(GetDocumentById), DateTime.UtcNow);
            return result;
        }
        public async Task<Document> CreateDocument(Document newDocument)
        {
            var result = await _data.Documents.AddAsync(newDocument);
            await _data.SaveChangesAsync();
            return result.Entity;
        }
    }
}
