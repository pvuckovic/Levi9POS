using Levi9.POS.Domain.DTOs.DocumentDTOs;

namespace Levi9.POS.Domain.Common.IDocument
{
    public interface IDocumentService
    {
        Task<DocumentDTO> GetDocumentById(int id);
        Task<Enum> CreateDocument(CreateDocumentDTO newDocument);
        Task<IEnumerable<DocumentSyncDto>> GetDocumentsByLastUpdate(string lastUpdate);
    }
}
