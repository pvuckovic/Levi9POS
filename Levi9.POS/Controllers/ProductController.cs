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
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, ILogger<ProductController> logger, IMapper mapper)
        {
            _productService = productService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetProductById(int id)
        {
            _logger.LogInformation("Entering {FunctionName} in ProductController. Timestamp: {Timestamp}.", nameof(GetProductById), DateTime.UtcNow);

            if (id <= 0)
            {
                _logger.LogError("Invalid product ID: {ProductId} in {FunctionName} of ProductController. Timestamp: {Timestamp}.", id, nameof(GetProductById), DateTime.UtcNow);
                return BadRequest("Id must be a positive integer");
            }

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product not found with ID: {ProductId} in {FunctionName} of ProductController. Timestamp: {Timestamp}.", id, nameof(GetProductById), DateTime.UtcNow);
                return NotFound($"Product with Id {id} not found");
            }

            var productResponse = _mapper.Map<ProductResponse>(product);
            _logger.LogInformation("Product retrieved successfully with ID: {ProductId} in {FunctionName} of ProductController. Timestamp: {Timestamp}.", id, nameof(GetProductById), DateTime.UtcNow);
            return Ok(productResponse);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SearchProducts([FromQuery] ProductSearchRequest request)
        {
            _logger.LogInformation("Entering {FunctionName} in ProductController. Timestamp: {Timestamp}.", nameof(SearchProducts), DateTime.UtcNow);

            if (request.Page <= 0)
            {
                _logger.LogError("Invalid Page: {Page} in {FunctionName} of ProductController. Timestamp: {Timestamp}.", request.Page, nameof(SearchProducts), DateTime.UtcNow);
                return BadRequest("The 'page' parameter must be greater than 0.");
            }
            if (!string.IsNullOrEmpty(request.OrderBy) && string.IsNullOrEmpty(request.Direction))
            {
                _logger.LogError("Direction must have value in {FunctionName} of ProductController. Timestamp: {Timestamp}.", nameof(SearchProducts), DateTime.UtcNow);
                return BadRequest("If OrderBy is not empty, you must enter Direction!");
            }
            var productsRequest = _mapper.Map<ProductSearchRequestDTO>(request);

            var products = await _productService.SearchProductsAsync(productsRequest);

            if (products == null || !products.Any())
            {
                _logger.LogWarning("No products were found in {FunctionName} of ProductController. Timestamp: {Timestamp}.", nameof(SearchProducts), DateTime.UtcNow);
                return NotFound("No products were found that match the search criteria.");
            }

            var response = new ProductSearchResponse
            {
                Items = _mapper.Map<IEnumerable<ProductResponse>>(products),
                Page = request.Page
            };
            _logger.LogInformation("Products found successfully in {FunctionName} of ProductController. Timestamp: {Timestamp}.", nameof(SearchProducts), DateTime.UtcNow);
            return Ok(response);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> InsertProduct([FromBody] ProductInsertRequest request)
        {
            _logger.LogInformation("Entering {FunctionName} in ProductController. Timestamp: {Timestamp}.", nameof(InsertProduct), DateTime.UtcNow);

            if (request == null)
            {
                _logger.LogError("Invalid request value in {FunctionName} of ProductController. Timestamp: {Timestamp}.", nameof(InsertProduct), DateTime.UtcNow);
                return BadRequest("Request cannot be null");
            }
            if (string.IsNullOrEmpty(request.Name))
            {
                _logger.LogError("Invalid name value in {FunctionName} of ProductController. Timestamp: {Timestamp}.", nameof(InsertProduct), DateTime.UtcNow);
                return BadRequest("Name is required");
            }
            var product = _mapper.Map<ProductInsertRequestDTO>(request);
            var insertedProduct = await _productService.InsertProductAsync(product);

            if (insertedProduct == null)
            {
                _logger.LogError("Failed to insert product in {FunctionName} of ProductController. Timestamp: {Timestamp}.", nameof(InsertProduct), DateTime.UtcNow);
                return BadRequest("Product name must be unique");
            }


            var response = _mapper.Map<ProductInsertResponse>(insertedProduct);
            _logger.LogInformation("Product created successfully in {FunctionName} of ProductController. Timestamp: {Timestamp}.", nameof(InsertProduct), DateTime.UtcNow);
            return CreatedAtAction(nameof(GetProductById), new { id = response.Id }, response);
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateRequest request)
        {
            _logger.LogInformation("Entering {FunctionName} in ProductController. Timestamp: {Timestamp}.", nameof(UpdateProduct), DateTime.UtcNow);
            if (request == null)
            {
                _logger.LogError("Invalid request value in {FunctionName} of ProductController. Timestamp: {Timestamp}.", nameof(UpdateProduct), DateTime.UtcNow);
                return BadRequest("Request cannot be null");
            }
            if (request.GlobalId == Guid.Empty)
            {
                _logger.LogError("Product with GlobalId must exist. GlobalId: {product.GlobalId}. Function: {functionName}. Timestamp: {timestamp}.", nameof(UpdateProduct), DateTime.UtcNow);
                return BadRequest("GlobalId is must be exist");
            }
            var product = await _productService.GetProductByGlobalIdAsync(request.GlobalId);            
            if (product == null)
            {
                _logger.LogError("Product not found with GlobalId: {GlobalId} in {FunctionName} of ProductController. Timestamp: {Timestamp}.", request.GlobalId, nameof(UpdateProduct), DateTime.UtcNow);
                return NotFound($"Product with GlobalId {request.GlobalId} not found");
            }
            _mapper.Map(request, product);
            var productUpdate = _mapper.Map<ProductUpdateRequestDTO>(product);
            var updatedProduct = await _productService.UpdateProductAsync(productUpdate);

            if (updatedProduct == null)
            {
                _logger.LogError("Filed to update product in {FunctionName} of ProductController. Timestamp: {Timestamp}.", nameof(UpdateProduct), DateTime.UtcNow);
                return StatusCode(500, "Failed to update product");
            }

            var response = _mapper.Map<ProductUpdateResponse>(updatedProduct);
            _logger.LogInformation("Product updated successfully in {FunctionName} of ProductController. Timestamp: {Timestamp}.", nameof(InsertProduct), DateTime.UtcNow);
            return Ok(response);
        }
        [HttpPost("snyc")]
        [Authorize]
        public async Task<IActionResult> SyncProducts(List<ProductSyncRequest> products)
        {
            _logger.LogInformation("Entering {FunctionName} in ProductController. Timestamp: {Timestamp}.", nameof(SyncProducts), DateTime.UtcNow);
            var newProducts = _mapper.Map<List<ProductSyncRequestDTO>>(products);
            string result = await _productService.SyncProducts(newProducts);
            if (result == null)
            {
                _logger.LogError("Filed to update products in {FunctionName} of ProductController. Timestamp: {Timestamp}.", nameof(SyncProducts), DateTime.UtcNow);
                return BadRequest("Update failed!");
            }
            _logger.LogInformation("Products updated successfully in {FunctionName} of ProductController. Timestamp: {Timestamp}.", nameof(SyncProducts), DateTime.UtcNow);
            return Ok(result);
        }
    }
}