using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Levi9.POS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {

        private readonly IClientService _productService;

        public ClientController(IClientService productService)
        {
            _productService = productService;
        }

        //[HttpPost]
        //public async Task<IActionResult> AddClient(AddClientDto addClientDto)
        //{

        //}
    }
}
