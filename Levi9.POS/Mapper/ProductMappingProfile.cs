using AutoMapper;
using Levi9.POS.Domain.DTOs.ProductDTOs;
using Levi9.POS.Domain.Models;
using Levi9.POS.WebApi.Request.ProductRequest;
using Levi9.POS.WebApi.Response.ProductResponse;
namespace Levi9.POS.WebApi.Mapper
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();
            CreateMap<ProductDTO, ProductResponse>();
            CreateMap<ProductSearchRequest, ProductSearchRequestDTO>();
        }
    }
}
