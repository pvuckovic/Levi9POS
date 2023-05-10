using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.Common.IDocument;
using Levi9.POS.Domain.Common.IProduct;
using Levi9.POS.Domain.DTOs.DocumentDTOs;
using Levi9.POS.Domain.Models;
using Levi9.POS.Domain.Models.Enum;

namespace Levi9.POS.Domain.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IProductRepository _productRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        public DocumentService(IDocumentRepository documentRepository, 
                               IProductRepository productRepository, 
                               IClientRepository clientRepository, 
                               IMapper mapper) 
        {
            _documentRepository = documentRepository;
            _productRepository = productRepository;
            _clientRepository = clientRepository;
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
            if (!await _clientRepository.DoesClientExist(newDocument.ClientId))
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
