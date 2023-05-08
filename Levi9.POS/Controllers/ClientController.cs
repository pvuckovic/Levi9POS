using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.WebApi.Response;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<AddClientResponse>> AddClient(AddClientDto addClientDto)
        {
            var newClient = await _clientService.AddClient(addClientDto);

            var addClientResponse = _mapper.Map<AddClientResponse>(newClient);

            return CreatedAtAction(nameof(addClientResponse), new { id = addClientResponse.Id }, addClientResponse);
        }
    }
}
