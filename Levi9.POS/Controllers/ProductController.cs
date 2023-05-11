using AutoMapper;
using Levi9.POS.Domain.Common.IProduct;
using Levi9.POS.Domain.DTOs.ProductDTOs;
using Levi9.POS.WebApi.Request.ProductRequest;
using Levi9.POS.WebApi.Response.ProductResponse;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> SearchProducts([FromQuery] ProductSearchRequest request)
        {
            if (request.Page <= 0)
            {
                return BadRequest("The 'page' parameter must be greater than 0.");
            }
            if(!string.IsNullOrEmpty(request.OrderBy) && string.IsNullOrEmpty(request.Direction))
            {
                return BadRequest("If OrderBy is not empty, you must enter Direction!");
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
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> InsertProduct([FromBody] ProductInsertRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null");
            }
            if (string.IsNullOrEmpty(request.Name))
            {
                return BadRequest("Name is required");
            }
            var product = _mapper.Map<ProductInsertRequestDTO>(request);
            var insertedProduct = await _productService.InsertProductAsync(product);

            if (insertedProduct == null)
            {
                return StatusCode(500, "Failed to insert product");
            }

           
            var response = _mapper.Map<ProductInsertResponse>(insertedProduct);
            return CreatedAtAction(nameof(GetProductById), new { id = response.Id }, response);
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null");
            }

            var product = await _productService.GetProductByGlobalIdAsync(request.GlobalId);

            if (product == null)
            {
                return NotFound($"Product with GlobalId {request.GlobalId} not found");
            }

            _mapper.Map(request, product);
            var productUpdate = _mapper.Map<ProductUpdateRequestDTO>(product);
            var updatedProduct = await _productService.UpdateProductAsync(productUpdate);

            if (updatedProduct == null)
            {
                return StatusCode(500, "Failed to update product");
            }

            var response = _mapper.Map<ProductUpdateResponse>(updatedProduct);
            return Ok(response);
        }
    }
}