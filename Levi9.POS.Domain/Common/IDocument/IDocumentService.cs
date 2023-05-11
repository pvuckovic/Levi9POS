using Levi9.POS.Domain.DTOs.DocumentDTOs;

namespace Levi9.POS.Domain.Common.IDocument
{
    public interface IDocumentService
    {
        public Task<DocumentDTO> GetDocumentById(int id);
        public Task<Enum> CreateDocument(CreateDocumentDTO newDocument);
    }
}
