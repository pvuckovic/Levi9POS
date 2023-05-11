using AutoMapper;
using Levi9.POS.Domain.DTOs.DocumentDTOs;
using Levi9.POS.Domain.Models;
using Levi9.POS.WebApi.Request.DocumentRequest;
using Levi9.POS.WebApi.Response.DocumentResponse;

namespace Levi9.POS.WebApi.Mapper
{
    public class DocumentMappingProfile : Profile
    {
        public DocumentMappingProfile()
        {
            //get document by id from repository map order
            CreateMap<Document, DocumentDTO>()
                .ForMember(dest => dest.DocumentItems, opt => opt.MapFrom(src => src.ProductDocuments));
            CreateMap<ProductDocument, DocumentItemDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.LastUpdate, opt => opt.MapFrom(src => src.Product.LastUpdate));
            CreateMap<DocumentDTO, GetByIdDocumentResponse>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.DocumentItems));
            CreateMap<DocumentItemDTO, GetByIdDocumentItemResponse>();
            //create document map order
            CreateMap<CreateDocumentRequest, CreateDocumentDTO>()
                .ForMember(dest => dest.DocumentItems, opt => opt.MapFrom(src => src.Items));
            CreateMap<CreateDocumentItemRequest, CreateDocumentItemDTO>();
            CreateMap<CreateDocumentDTO, Document>()
                .ForMember(dest => dest.ProductDocuments, opt => opt.MapFrom(src => src.DocumentItems));
            CreateMap<CreateDocumentItemDTO, ProductDocument>();
        }
    }
}
