using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Models;

namespace Levi9.POS.Domain.Service
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;


        public ClientService(IClientRepository clientRepository, IMapper mapper, IAuthenticationService authenticationService)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        public async Task<AddClientDto> AddClient(AddClientDto addClientDto)
        {
            addClientDto.GlobalId = Guid.NewGuid();
            addClientDto.Salt = _authenticationService.GenerateRandomSalt();
            addClientDto.PasswordHash = _authenticationService.HashPassword(addClientDto.PasswordHash, addClientDto.Salt);
            addClientDto.LastUpdate = DateTime.Now.ToFileTimeUtc().ToString();

            _clientRepository.AddClient(addClientDto);

            return addClientDto;
        }


    }
}
