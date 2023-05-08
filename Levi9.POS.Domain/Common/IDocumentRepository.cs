using Levi9.POS.Domain.Models;

namespace Levi9.POS.Domain.Common
{
    public interface IDocumentRepository
    {
        public Task<Document> GetDocumentById(int id);
    }
}
