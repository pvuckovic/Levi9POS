using AutoMapper;
using Levi9.POS.Domain.Common.IDocument;
using Levi9.POS.Domain.DTOs.DocumentDTOs;
using Levi9.POS.Domain.Models.Enum;
using Levi9.POS.WebApi.Request.DocumentRequest;
using Levi9.POS.WebApi.Response.DocumentResponse;
using Microsoft.AspNetCore.Mvc;

namespace Levi9.POS.WebApi.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> GetDocumentById(int documentId)
        {
            _logger.LogInformation("Entering {FunctionName} in DocumentController.", nameof(GetDocumentById));
            if (documentId < 1)
            {
                _logger.LogError("Invalid document ID: {DocumentId} in {FunctionName} of DocumentController. Timestamp: {Timestamp}. Request ID: {RequestId}.", documentId, nameof(GetDocumentById), DateTime.UtcNow, Request.HttpContext.TraceIdentifier);
                return BadRequest("The ID must be a positive number.");
            }

            var document = await _documentService.GetDocumentById(documentId);
            if (document == null)
            {
                _logger.LogWarning("Document not found with this ID: {DocumentId} in {FunctionName} of DocumentController. Timestamp: {Timestamp}. Request ID: {RequestId}.", documentId, nameof(GetDocumentById), DateTime.UtcNow, Request.HttpContext.TraceIdentifier);
                return NotFound("There is no document with the desired ID.");
            }

            var result = _mapper.Map<GetByIdDocumentResponse>(document);
            _logger.LogInformation("Document retrieved successfully with this ID: {DocumentId} in {FunctionName} of DocumentController. Timestamp: {Timestamp}. Request ID: {RequestId}.", documentId, nameof(GetDocumentById), DateTime.UtcNow, Request.HttpContext.TraceIdentifier);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDocument(CreateDocumentRequest newDocument)
        {
            var document = _mapper.Map<CreateDocumentDTO>(newDocument);
            var result = await _documentService.CreateDocument(document);

            CreateDocumentResult resultEnum = (CreateDocumentResult)result;
            if (resultEnum == CreateDocumentResult.ClientNotFound)
                return BadRequest("Client does not exist!");
            else if (resultEnum == CreateDocumentResult.ProductNotFound)
                return BadRequest("Product does not exist!");
            return Ok("Document created successfully");
        }
    }
}
