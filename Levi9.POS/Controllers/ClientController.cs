using AutoMapper;
using Levi9.POS.Domain.Common.IClient;
using Levi9.POS.Domain.DTOs.ClientDTOs;
using Levi9.POS.WebApi.Request.ClientRequests;
using Levi9.POS.WebApi.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Levi9.POS.WebApi.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {

        private readonly IClientService _clientService;
        private readonly ILogger<ClientController> _logger;
        private readonly IMapper _mapper;
        public ClientController(IClientService clientService, ILogger<ClientController> logger, IMapper mapper)
        {
            _clientService = clientService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ClientResponse>> AddClient(ClientRequest clientRequest)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientController. Timestamp: {Timestamp}.", nameof(AddClient), DateTime.UtcNow);
            if (_clientService.CheckEmailExist(clientRequest.Email))
            {
                _logger.LogError("Client already exists with Email: {Email} in {FunctionName} of ClientController. Timestamp: {Timestamp}.", clientRequest.Email, nameof(UpdateClient), DateTime.UtcNow);
                return BadRequest("Email already exists!");
            }
            ClientDto clientMap = _mapper.Map<ClientDto>(clientRequest);
            ClientDto clientDto = await _clientService.AddClient(clientMap);
            _logger.LogInformation("Client created successfully in {FunctionName} of ClientController. Timestamp: {Timestamp}.", nameof(AddClient), DateTime.UtcNow);
            return Ok(_mapper.Map<ClientResponse>(clientDto));
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetClientById(int id)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientController. Timestamp: {Timestamp}.", nameof(GetClientById), DateTime.UtcNow);
            if (id <= 0)
            {
                _logger.LogError("Invalid client ID: {ClientId} in {FunctionName} of ClientController. Timestamp: {Timestamp}.", id, nameof(GetClientById), DateTime.UtcNow);
                return BadRequest("Id must be a positive number");
            }

            var client = await _clientService.GetClientById(id);
            if (client == null)
            {
                _logger.LogWarning("Client not found with ID: {ClientId} in {FunctionName} of ClientController. Timestamp: {Timestamp}.", id, nameof(GetClientById), DateTime.UtcNow);
                return NotFound($"Client with Id {id} not found");
            }
            
            _logger.LogInformation("Client retrieved successfully with ID: {ClientId} in {FunctionName} of ClientController. Timestamp: {Timestamp}.", id, nameof(GetClientById), DateTime.UtcNow);
            var clientResponse = _mapper.Map<ClientResponse>(client);
            return Ok(clientResponse);
        }
        [HttpGet("global/{id}")]
        [Authorize]
        public async Task<IActionResult> GetClientByGlobalId(Guid id)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientController. Timestamp: {Timestamp}.", nameof(GetClientByGlobalId), DateTime.UtcNow);

            var client = await _clientService.GetClientByGlobalId(id);
            if (client == null)
            {
                _logger.LogWarning("Client not found with ID: {ClientId} in {FunctionName} of ClientController. Timestamp: {Timestamp}.", id, nameof(GetClientByGlobalId), DateTime.UtcNow);
                return NotFound($"Client with Id {id} not found");
            }
            _logger.LogInformation("Client retrieved successfully with GlobalId: {ClientGlobalId} in {FunctionName} of ClientController. Timestamp: {Timestamp}.", id, nameof(GetClientByGlobalId), DateTime.UtcNow);
            var clientResponse = _mapper.Map<ClientResponse>(client);
            return Ok(clientResponse);
        }
        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateClient(ClientUpdate clientUpdate)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientController. Timestamp: {Timestamp}.", nameof(UpdateClient), DateTime.UtcNow);

            if (_clientService.CheckEmailExist(clientUpdate.Email))
            {
                _logger.LogError("Client already exists with Email: {Email} in {FunctionName} of ClientController. Timestamp: {Timestamp}.", clientUpdate.Email, nameof(UpdateClient), DateTime.UtcNow);
                return BadRequest("Email already exists!");
            }

            var clientMap = _mapper.Map<UpdateClientDto>(clientUpdate);
            clientMap = await _clientService.UpdateClient(clientMap);

            if(clientMap == null)
            {
                _logger.LogError("Failed to update client in {FunctionName} of ClientController. Timestamp: {Timestamp}.", nameof(UpdateClient), DateTime.UtcNow);
                return BadRequest();
            }
            _logger.LogInformation("Client updated successfully in {FunctionName} of ClientController. Timestamp: {Timestamp}.", nameof(UpdateClient), DateTime.UtcNow);
            var clientResponse = _mapper.Map<ClientResponse>(clientMap);
            return Ok(clientResponse);
        }
        [AllowAnonymous]
        [HttpGet("sync/{lastUpdate}")]
        public async Task<IActionResult> GetAllClients(string lastUpdate)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientController. Timestamp: {Timestamp}.", nameof(GetAllClients), DateTime.UtcNow);
            var clients = await _clientService.GetClientsByLastUpdate(lastUpdate);
            if (!clients.Any())
            {
                _logger.LogWarning("Clients not found in {FunctionName} of ClientController. Timestamp: {Timestamp}.", nameof(GetAllClients), DateTime.UtcNow);
                return Ok(clients);
            }
            var mappedClients = clients.Select(c => _mapper.Map<ClientResponse>(c));
            _logger.LogInformation("Clients retrieved successfully in {FunctionName} of ClientController. Timestamp: {Timestamp}.", nameof(GetAllClients), DateTime.UtcNow);
            return Ok(mappedClients);
        }
        [AllowAnonymous]
        [HttpPost("sync")]
        public async Task<IActionResult> SyncClients([FromBody]ClientsSyncRequest syncRequest)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientController. Timestamp: {Timestamp}.", nameof(SyncClients), DateTime.UtcNow);
            var clientsDto = _mapper.Map<ClientsSyncDto>(syncRequest);
            var result = await _clientService.SyncClients(clientsDto);
            var clientResponse = _mapper.Map<ClientsSyncResponse>(result);
            _logger.LogInformation("Clients synced successfully in {FunctionName} of ClientController. Timestamp: {Timestamp}.", nameof(SyncClients), DateTime.UtcNow);
            return Ok(clientResponse);
        }
    }
}
