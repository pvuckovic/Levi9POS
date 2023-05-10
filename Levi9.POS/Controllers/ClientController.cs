﻿using AutoMapper;
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
            ClientDto clientMap = _mapper.Map<ClientDto>(clientRequest);
            ClientDto clientDto = await _clientService.AddClient(clientMap);

            return Ok(_mapper.Map<ClientResponse>(clientDto));
        }
        [HttpGet("getclientbyid/{id}")]
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
        [HttpGet("getclientbyglobalid/{globalId}")]
        public async Task<IActionResult> GetClientByGlobalId(Guid globalId)
        {
            var client = await _clientService.GetClientByGlobalId(globalId);
            if (client == null)
            {
                return NotFound($"Client with Id {globalId} not found");
            }
            var clientResponse = _mapper.Map<ClientResponse>(client);
            return Ok(clientResponse);
        }
    }
}
