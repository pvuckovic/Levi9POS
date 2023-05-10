using Levi9.POS.Domain.Models;

namespace Levi9.POS.Domain.Common.IDocument
{
    public interface IDocumentRepository
    {
        public Task<Document> GetDocumentById(int id);
        public Task<Document> CreateDocument(Document newDocument);
        // This will be in Clinet repository, but for now
        public Task<bool> DoesClientExist(int clientId);
    }
}
