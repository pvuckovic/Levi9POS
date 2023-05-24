using AutoMapper;
using Levi9.POS.Domain.Common.IDocument;
using Levi9.POS.Domain.DTOs.ClientDTOs;
using Levi9.POS.Domain.DTOs.DocumentDTOs;
using Levi9.POS.Domain.Models.Enum;
using Levi9.POS.WebApi.Request.ClientRequests;
using Levi9.POS.WebApi.Request.DocumentRequest;
using Levi9.POS.WebApi.Response;
using Levi9.POS.WebApi.Response.DocumentResponse;
using Levi9.POS.WebApi.Response.ProductResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Levi9.POS.WebApi.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IMapper _mapper;
        private readonly ILogger<DocumentController> _logger;
        public DocumentController(IDocumentService documentService, ILogger<DocumentController> logger, IMapper mapper)
        {
            _documentService = documentService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{documentId}")]
        [Authorize]
        public async Task<IActionResult> GetDocumentById(int documentId)
        {
            _logger.LogInformation("Entering {FunctionName} in DocumentController. Timestamp: {Timestamp}.", nameof(GetDocumentById), DateTime.UtcNow);
            if (documentId < 1)
            {
                _logger.LogError("Invalid document ID: {DocumentId} in {FunctionName} of DocumentController. Timestamp: {Timestamp}.", documentId, nameof(GetDocumentById), DateTime.UtcNow);
                return BadRequest("The ID must be a positive number.");
            }

            var document = await _documentService.GetDocumentById(documentId);
            if (document == null)
            {
                _logger.LogWarning("Document not found with ID: {DocumentId} in {FunctionName} of DocumentController. Timestamp: {Timestamp}.", documentId, nameof(GetDocumentById), DateTime.UtcNow);
                return NotFound("There is no document with the desired ID.");
            }

            var result = _mapper.Map<GetByIdDocumentResponse>(document);
            _logger.LogInformation("Document retrieved successfully with ID: {DocumentId} in {FunctionName} of DocumentController. Timestamp: {Timestamp}.", documentId, nameof(GetDocumentById), DateTime.UtcNow);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateDocument(CreateDocumentRequest newDocument)
        {
            _logger.LogInformation("Entering {FunctionName} in DocumentController. Timestamp: {Timestamp}.", nameof(CreateDocument), DateTime.UtcNow);
            var document = _mapper.Map<CreateDocumentDTO>(newDocument);
            var result = await _documentService.CreateDocument(document);

            CreateDocumentResult resultEnum = (CreateDocumentResult)result;
            if (resultEnum == CreateDocumentResult.ClientNotFound)
            {
                _logger.LogError("Invalid document ID: {ClientId} in {FunctionName} of DocumentController. Timestamp: {Timestamp}.", newDocument.ClientId, nameof(CreateDocument), DateTime.UtcNow);
                return BadRequest("Client does not exist!");
            }
            else if (resultEnum == CreateDocumentResult.ProductNotFound)
            {
                _logger.LogError("Invalid product ID in {FunctionName} of DocumentController. Timestamp: {Timestamp}.", nameof(CreateDocument), DateTime.UtcNow);
                return BadRequest("Product does not exist!");
            }
            _logger.LogInformation("Document created successfully in {FunctionName} of DocumentController. Timestamp: {Timestamp}.", nameof(CreateDocument), DateTime.UtcNow);
            return Ok("Document created successfully");
        }
        [HttpGet("sync/{lastUpdate}")]
        public async Task<IActionResult> GetAllDocuments(string lastUpdate)
        {
            _logger.LogInformation("Entering {FunctionName} in DocumentController. Timestamp: {Timestamp}.", nameof(GetAllDocuments), DateTime.UtcNow);
            var documents = await _documentService.GetDocumentsByLastUpdate(lastUpdate);

            if (!documents.Any())
            {
                _logger.LogInformation("There is no documents to sync in database DocumentController. Timestamp: {Timestamp}.", nameof(GetAllDocuments), DateTime.UtcNow);
                return Ok(documents);
            }

            var mappedDocuments = documents.Select(p => _mapper.Map<DocumentSyncResponse>(p));
            _logger.LogInformation("Products retrieved successfully in {FunctionName} of DocumentController. Timestamp: {Timestamp}.", nameof(GetAllDocuments), DateTime.UtcNow);
            return Ok(mappedDocuments);
        }
    }
}
