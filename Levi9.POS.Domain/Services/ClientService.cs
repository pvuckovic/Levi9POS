using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Helpers;

namespace Levi9.POS.Domain.Service
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;


        public ClientService(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<AddClientDto> AddClient(AddClientDto addClientDto)
        {
            addClientDto.GlobalId = Guid.NewGuid();
            addClientDto.Salt = AuthenticationHelper.GenerateRandomSalt();
            addClientDto.PasswordHash = AuthenticationHelper.HashPassword(addClientDto.PasswordHash, addClientDto.Salt);
            addClientDto.LastUpdate = DateTime.Now.ToFileTimeUtc().ToString();

            _clientRepository.AddClient(addClientDto);

            return addClientDto;
        }


    }
}
