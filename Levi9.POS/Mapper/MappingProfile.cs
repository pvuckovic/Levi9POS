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
            CreateMap<Document, DocumentDTO>()
                .ForMember(dest => dest.DocumentItems, opt => opt.MapFrom(src => src.ProductDocuments))
                .ReverseMap();
            CreateMap<ProductDocument, DocumentItemDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.LastUpdate, opt => opt.MapFrom(src => src.Product.LastUpdate))
                .ReverseMap();
            CreateMap<DocumentDTO, DocumentResponse>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.DocumentItems))
                .ReverseMap();
            CreateMap<DocumentItemDTO, DocumentItemResponse>().ReverseMap();
            CreateMap<ProductSearchRequest,ProductSearchRequestDTO>();
        }
    }
}
