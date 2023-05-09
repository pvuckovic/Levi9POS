using AutoMapper;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Models;
using Levi9.POS.WebApi.Request;
using Levi9.POS.WebApi.Response;

namespace Levi9.POS.WebApi.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, ProductResponse>();
            CreateMap<ProductSearchRequest,ProductSearchRequestDTO>();
        }
    }
}
