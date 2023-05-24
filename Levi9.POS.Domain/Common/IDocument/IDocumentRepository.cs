using Levi9.POS.Domain.Models;

namespace Levi9.POS.Domain.Common.IDocument
{
    public interface IDocumentRepository
    {
        Task<Document> GetDocumentById(int id);
        Task<Document> CreateDocument(Document newDocument);
        Task<IEnumerable<Document>> GetDocumentsByLastUpdate(string lastUpdate);
    }
}
