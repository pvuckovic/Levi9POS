using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Models;
using Levi9.POS.Domain.Models.Enum;

namespace Levi9.POS.Domain.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public DocumentService(IDocumentRepository documentRepository, IProductRepository productRepository, IMapper mapper) 
        {
            _documentRepository = documentRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<DocumentDTO> GetDocumentById(int id)
        {
            var document = await _documentRepository.GetDocumentById(id);
            if (document == null)
                return null;

            var response = _mapper.Map<DocumentDTO>(document);
            return response;
        }

        public async Task<Enum> CreateDocument(CreateDocumentDTO newDocument)
        {
            if (!await _documentRepository.DoesClientExist(newDocument.ClientId))
                return CreateDocumentResult.ClientNotFound;

            foreach (var productDocument in newDocument.DocumentItems)
            {
                if (!await _productRepository.DoesProductExist(productDocument.ProductId, productDocument.Name))
                    return CreateDocumentResult.ProductNotFound;
            }

            var document = _mapper.Map<Document>(newDocument);
            var date = DateTime.Now.ToFileTimeUtc().ToString();
            document.CreationDay = date;
            document.LastUpdate = date;
            document.GlobalId = Guid.NewGuid();
            await _documentRepository.CreateDocument(document);
            return CreateDocumentResult.Success;
        }
    }
}
