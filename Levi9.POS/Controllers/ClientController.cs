using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.WebApi.Request;
using Levi9.POS.WebApi.Response;
using Microsoft.AspNetCore.Mvc;

namespace Levi9.POS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {

        private readonly IClientService _clientService;
        private readonly IMapper _mapper;
        public ClientController(IClientService clientService, IMapper mapper)
        {
            _clientService = clientService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ClientResponse>> AddClient(ClientRequest clientRequest)
        {
            AddClientDto clientMap = _mapper.Map<AddClientDto>(clientRequest);
            AddClientDto clientDto = await _clientService.AddClient(clientMap);

            return Ok(_mapper.Map<ClientResponse>(clientDto));
        }
    }
}
