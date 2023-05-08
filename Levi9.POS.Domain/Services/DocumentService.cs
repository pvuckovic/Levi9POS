using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;

namespace Levi9.POS.Domain.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _repo;
        private readonly IMapper _mapper;
        public DocumentService(IDocumentRepository repository, IMapper mapper) 
        {

            _repo = repository;
            _mapper = mapper;
        }

        public async Task<DocumentDTO> GetDocumentById(int id)
        {
            var document = await _repo.GetDocumentById(id);
            if (document == null)
                return null;

            var response = _mapper.Map<DocumentDTO>(document);
            return response;
        }
    }
}
