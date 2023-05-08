using AutoMapper;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Models;
using Levi9.POS.WebApi.Responses;

namespace Levi9.POS.WebApi.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile() 
        {
            ConfigureMappings();
        }

        private void ConfigureMappings()
        {
            CreateMap<Product,ProductDTO>();
            CreateMap<ProductDTO, ProductResponse>();
        }
    }
}
