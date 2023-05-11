using AutoMapper;
using Levi9.POS.Domain.Common.IClient;
using Levi9.POS.Domain.DTOs.ClientDTOs;
using Levi9.POS.WebApi.Request.ClientRequests;
using Levi9.POS.WebApi.Response;
using Microsoft.AspNetCore.Authorization;
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
            ClientDto clientMap = _mapper.Map<ClientDto>(clientRequest);
            ClientDto clientDto = await _clientService.AddClient(clientMap);

            return Ok(_mapper.Map<ClientResponse>(clientDto));
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetClientById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be a positive integer");
            }

            var client = await _clientService.GetClientById(id);
            if (client == null)
            {
                return NotFound($"Client with Id {id} not found");
            }

            var clientResponse = _mapper.Map<ClientResponse>(client);
            return Ok(clientResponse);
        }
        [HttpGet("global/{id}")]
        [Authorize]
        public async Task<IActionResult> GetClientByGlobalId(Guid id)
        {
            var client = await _clientService.GetClientByGlobalId(id);
            if (client == null)
            {
                return NotFound($"Client with Id {id} not found");
            }
            var clientResponse = _mapper.Map<ClientResponse>(client);
            return Ok(clientResponse);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateClient(ClientUpdate clientUpdate)
        {
            if (_clientService.CheckEmailExist(clientUpdate.Email))
                return BadRequest("Email already exists!");

            var clientMap = _mapper.Map<UpdateClientDto>(clientUpdate);
            clientMap = await _clientService.UpdateClient(clientMap);

            if(clientMap == null)
            {
                return BadRequest();
            }

            var clientResponse = _mapper.Map<ClientResponse>(clientMap);
            return Ok(clientResponse);
        }
    }
}
