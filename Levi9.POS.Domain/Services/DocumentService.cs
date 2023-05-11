using AutoMapper;
using Azure.Core;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.Common.IDocument;
using Levi9.POS.Domain.Common.IProduct;
using Levi9.POS.Domain.DTOs.DocumentDTOs;
using Levi9.POS.Domain.Models;
using Levi9.POS.Domain.Models.Enum;
using Microsoft.Extensions.Logging;

namespace Levi9.POS.Domain.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IProductRepository _productRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<DocumentService> _logger;
        private readonly IMapper _mapper;
        public DocumentService(IDocumentRepository documentRepository, 
                               IProductRepository productRepository, 
                               IClientRepository clientRepository, 
                               ILogger<DocumentService> logger,
                               IMapper mapper) 
        {
            _documentRepository = documentRepository;
            _productRepository = productRepository;
            _clientRepository = clientRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<DocumentDTO> GetDocumentById(int id)
        {
            _logger.LogInformation("Entering {FunctionName} in DocumentService.", nameof(GetDocumentById));
            var document = await _documentRepository.GetDocumentById(id);
            if (document == null)
            {
                _logger.LogWarning("Document not found with this ID: {DocumentId} in {FunctionName} of DocumentService. Timestamp: {Timestamp}.", id, nameof(GetDocumentById), DateTime.UtcNow);
                return null;
            }

            var response = _mapper.Map<DocumentDTO>(document);
            _logger.LogInformation("Document retrieved successfully with this ID: {DocumentId} in {FunctionName} of DocumentService. Timestamp: {Timestamp}.", id, nameof(GetDocumentById), DateTime.UtcNow);
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
