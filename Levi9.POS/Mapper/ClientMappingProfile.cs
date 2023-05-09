﻿using AutoMapper;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Models;
using Levi9.POS.WebApi.Request;
using Levi9.POS.WebApi.Response;

namespace Levi9.POS.WebApi.Mappings
{
    public class ClientMappingProfile : Profile
    {
        public ClientMappingProfile()
        {
            CreateMap<ClientRequest, AddClientDto>();
            CreateMap<AddClientDto, ClientResponse>();
            CreateMap<AddClientDto, Client>();
            CreateMap<Client, AddClientDto>();

        }
    }
}