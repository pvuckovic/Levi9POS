using Levi9.POS.Domain.DTOs;

namespace Levi9.POS.Domain.Common
{
    public interface IDocumentService
    {
        public Task<DocumentDTO> GetDocumentById(int id);
        public Task<Enum> CreateDocument(CreateDocumentDTO newDocument);
    }
}
