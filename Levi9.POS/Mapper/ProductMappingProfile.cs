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
            CreateMap<ProductInsertRequest, ProductInsertRequestDTO>();
            CreateMap<ProductInsertRequestDTO, Product>()
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.ProductImageUrl));
            CreateMap<ProductDTO, ProductInsertResponse>();
            CreateMap<ProductInsertRequest, ProductDTO>();
            CreateMap<ProductUpdateRequest, ProductDTO>();
            CreateMap<ProductDTO, ProductUpdateRequestDTO>();
            CreateMap<Product, ProductUpdateResponse>();
            CreateMap<ProductUpdateRequestDTO, Product>()
                .ForMember(dest => dest.GlobalId, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdate, opt => opt.MapFrom(src => DateTime.Now.ToFileTimeUtc().ToString()))
                .ForMember(dest => dest.ProductImageUrl, opt => opt.Ignore());
            CreateMap<ProductDTO, ProductUpdateResponse>();
            //sync path
            CreateMap<ProductSyncRequest, ProductSyncRequestDTO>();
            CreateMap<ProductSyncRequestDTO, Product>();
        }
    }
}
