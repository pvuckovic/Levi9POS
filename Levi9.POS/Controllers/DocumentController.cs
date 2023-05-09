using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Models.Enum;
using Levi9.POS.WebApi.Request;
using Levi9.POS.WebApi.Response;
using Microsoft.AspNetCore.Mvc;

namespace Levi9.POS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IMapper _mapper;
        public DocumentController(IDocumentService documentService, IMapper mapper)
        {
            _documentService = documentService;
            _mapper = mapper;
        }

        [HttpGet("{documentId}")]
        public async Task<IActionResult> GetDocumentById(int documentId)
        {
            if (documentId < 1)
                return BadRequest("The ID must be a positive number.");

            var document = await _documentService.GetDocumentById(documentId);
            if (document == null)
                return NotFound("There is no document with the desired ID.");

            var result = _mapper.Map<DocumentResponse>(document);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDocument(DocumentRequest newDocument)
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
