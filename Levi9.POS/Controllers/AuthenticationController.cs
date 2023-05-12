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
        private readonly ILogger<AuthenticationController> _logger;
        private readonly JwtOptions _config;
        public AuthenticationController(IClientService clientService, ILogger<AuthenticationController> logger, JwtOptions config)
        {
            _clientService = clientService;
            _config = config;
            _logger = logger;

        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ClientAuthentication(ClientLogin clientLogin)
        {
            _logger.LogInformation("Entering {FunctionName} in AuthenticationController. Timestamp: {Timestamp}.", nameof(ClientAuthentication), DateTime.UtcNow);
            ClientDto clientDto = await _clientService.GetClientByEmail(clientLogin.Email);
            if (clientDto == null)
            {
                _logger.LogWarning("Invalid emaila address: {Email} in {FunctionName} of AuthenticationController. Timestamp: {Timestamp}.", clientLogin.Email, nameof(ClientAuthentication), DateTime.UtcNow);
                return BadRequest("Email not valid");
            }
            bool validateUser = AuthenticationHelper.Validate(clientDto.Password, clientDto.Salt, clientLogin.Password);
            if (!validateUser)
            {
                _logger.LogWarning("Wront password in {FunctionName} of AuthenticationController. Timestamp: {Timestamp}.", nameof(ClientAuthentication), DateTime.UtcNow);
                return BadRequest("Bad Request - wrong password");
            }
            var token = AuthenticationHelper.GenerateJwt(_config);
            _logger.LogInformation("Client successfully validated in {FunctionName} of AuthenticationController. Timestamp: {Timestamp}.", nameof(ClientAuthentication), DateTime.UtcNow);
            return Ok(token);
        }
    }
}
