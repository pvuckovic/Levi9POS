using AutoMapper;
using Levi9.POS.Domain.DTOs.ClientDTOs;
using Levi9.POS.Domain.Models;
using Levi9.POS.WebApi.Request.ClientRequests;
using Levi9.POS.WebApi.Response;

namespace Levi9.POS.WebApi.Mappings
{
    public class ClientMappingProfile : Profile
    {
        public ClientMappingProfile()
        {
            CreateMap<ClientRequest, ClientDto>();
            CreateMap<ClientDto, ClientResponse>();
            CreateMap<ClientDto, Client>();
            CreateMap<Client, ClientDto>();
            CreateMap<ClientUpdate, UpdateClientDto>();
            CreateMap<UpdateClientDto, Client>();
            CreateMap<UpdateClientDto, ClientResponse>();
            CreateMap<Client, UpdateClientDto>();
            CreateMap<ClientSyncRequest, ClientSyncDto>();
            CreateMap<ClientsSyncRequest, ClientsSyncDto>();
            CreateMap<ClientSyncDto, Client>();
            CreateMap<Client, ClientSyncDto>();
            CreateMap<ClientSyncDto, ClientSyncResponse>();
            CreateMap<ClientsSyncDto, ClientsSyncResponse>();
        }
    }
}
