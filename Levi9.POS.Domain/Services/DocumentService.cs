using AutoMapper;
using Levi9.POS.Domain.Common.IClient;
using Levi9.POS.Domain.Common.IDocument;
using Levi9.POS.Domain.Common.IProduct;
using Levi9.POS.Domain.DTOs.DocumentDTOs;
using Levi9.POS.Domain.DTOs.ProductDTOs;
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
            _logger.LogInformation("Entering {FunctionName} in DocumentService. Timestamp: {Timestamp}.", nameof(GetDocumentById), DateTime.UtcNow);
            var document = await _documentRepository.GetDocumentById(id);
            if (document == null)
            {
                _logger.LogWarning("Document not found with ID: {DocumentId} in {FunctionName} of DocumentService. Timestamp: {Timestamp}.", id, nameof(GetDocumentById), DateTime.UtcNow);
                return null;
            }

            var response = _mapper.Map<DocumentDTO>(document);
            _logger.LogInformation("Document retrieved successfully with ID: {DocumentId} in {FunctionName} of DocumentService. Timestamp: {Timestamp}.", id, nameof(GetDocumentById), DateTime.UtcNow);
            return response;
        }

        public async Task<Enum> CreateDocument(CreateDocumentDTO newDocument)
        {
            _logger.LogInformation("Entering {FunctionName} in DocumentService. Timestamp: {Timestamp}.", nameof(CreateDocument), DateTime.UtcNow);
            if (!await _clientRepository.DoesClientExist(newDocument.ClientId))
            {
                _logger.LogError("Invalid client ID: {ClientId} in {FunctionName} of DocumentService. Timestamp: {Timestamp}.", newDocument.ClientId, nameof(CreateDocument), DateTime.UtcNow);
                return CreateDocumentResult.ClientNotFound;
            }

            foreach (var productDocument in newDocument.DocumentItems)
            {
                if (!await _productRepository.DoesProductExist(productDocument.ProductId, productDocument.Name))
                {
                    _logger.LogError("Invalid product ID: {ProductId} in {FunctionName} of DocumentService. Timestamp: {Timestamp}.", productDocument.ProductId, nameof(CreateDocument), DateTime.UtcNow);
                    return CreateDocumentResult.ProductNotFound;
                }
            }
            var document = _mapper.Map<Document>(newDocument);
            var date = DateTime.Now.ToFileTimeUtc().ToString();
            document.CreationDay = date;
            document.LastUpdate = date;
            document.GlobalId = Guid.NewGuid();
            await _documentRepository.CreateDocument(document);
            _logger.LogInformation("Document created successfully in {FunctionName} of DocumentService. Timestamp: {Timestamp}.", nameof(CreateDocument), DateTime.UtcNow);
            return CreateDocumentResult.Success;
        }

        public async Task<IEnumerable<DocumentSyncDto>> GetDocumentsByLastUpdate(string lastUpdate)
        {
            _logger.LogInformation("Entering {FunctionName} in DocumentService. Timestamp: {Timestamp}.", nameof(GetDocumentsByLastUpdate), DateTime.UtcNow);
            var documents = await _documentRepository.GetDocumentsByLastUpdate(lastUpdate);
            if (!documents.Any())
                return new List<DocumentSyncDto>();
            _logger.LogInformation("Retrieving documents in {FunctionName} of DocumentService. Timestamp: {Timestamp}.", nameof(GetDocumentsByLastUpdate), DateTime.UtcNow);
            var mappedDocuments = documents.Select(p => _mapper.Map<DocumentSyncDto>(p));
            return mappedDocuments;
        }
    }
}
