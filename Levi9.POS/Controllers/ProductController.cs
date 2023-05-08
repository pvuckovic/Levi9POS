﻿using AutoMapper;
using Levi9.POS.Domain;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Models;
using Levi9.POS.WebApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var productResponse = _mapper.Map<ProductResponse>(product);
            return Ok(productResponse);
        }
    }
}