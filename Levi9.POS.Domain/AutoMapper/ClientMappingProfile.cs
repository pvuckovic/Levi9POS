using AutoMapper;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levi9.POS.Domain.AutoMapper
{
    internal class ClientMappingProfile : Profile
    {
        public ClientMappingProfile() 
        {
            CreateMap<AddClientDto, Client>();
        }
    }
}
