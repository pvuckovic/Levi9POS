using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Levi9.POS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AuthenticationController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(ClientLogin clientLogin)
        {
            bool auth = await _loginService.ValidateClient(clientLogin);
            if (auth)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Wrong email or password");
            }
        }
    }
}
