using Levi9.POS.Domain.Common.IClient;
using Levi9.POS.Domain.DTOs.ClientDTOs;
using Levi9.POS.Domain.Helpers;
using Levi9.POS.WebApi.Request.ClientRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Levi9.POS.WebApi.Controllers
{

    [Route("v1/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly JwtOptions _config;
        public AuthenticationController(IClientService clientService, JwtOptions config)
        {
            _clientService = clientService;
            _config = config;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ClientAuthentication(ClientLogin clientLogin)
        {
            ClientDto clientDto = await _clientService.GetClientByEmail(clientLogin.Email);
            if (clientDto == null)
            {
                return BadRequest("Email not valid");
            }
            bool validateUser = AuthenticationHelper.Validate(clientDto.Password, clientDto.Salt, clientLogin.Password);
            if (!validateUser)
            {
                return BadRequest("Bad Request - wrong password");
            }
            var token = AuthenticationHelper.GenerateJwt(_config);
            return Ok(token);
        }
    }
}
