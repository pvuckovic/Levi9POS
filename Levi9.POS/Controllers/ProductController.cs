using AutoMapper;
using Levi9.POS.Domain.Common.IProduct;
using Levi9.POS.Domain.DTOs.ProductDTOs;
using Levi9.POS.WebApi.Request.ProductRequest;
using Levi9.POS.WebApi.Response.ProductResponse;
using Microsoft.AspNetCore.Mvc;
namespace Levi9.POS.WebApi.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be a positive integer");
            }

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound($"Product with Id {id} not found");
            }

            var productResponse = _mapper.Map<ProductResponse>(product);
            return Ok(productResponse);
        }
        [HttpGet]
        public async Task<IActionResult> SearchProducts([FromQuery] ProductSearchRequest request)
        {
            if (request.Page <= 0)
            {
                return BadRequest("The 'page' parameter must be greater than 0.");
            }
            var productsRequest = _mapper.Map<ProductSearchRequestDTO>(request);

            var products = await _productService.SearchProductsAsync(productsRequest);

            if (products == null || !products.Any())
            {
                return NotFound("No products were found that match the search criteria.");
            }

            var response = new ProductSearchResponse
            {
                Items = _mapper.Map<IEnumerable<ProductResponse>>(products),
                Page = request.Page
            };

            return Ok(response);
        }        
    }
}