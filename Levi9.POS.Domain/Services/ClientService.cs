using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Helpers;

namespace Levi9.POS.Domain.Services
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
        public async Task<ClientDto> AddClient(ClientDto addClientDto)
        {
            addClientDto.GlobalId = Guid.NewGuid();
            string salt = AuthenticationHelper.GenerateRandomSalt();
            addClientDto.Password = AuthenticationHelper.HashPassword(addClientDto.Password, salt);
            addClientDto.Salt = salt;
            addClientDto.LastUpdate = DateTime.Now.ToFileTimeUtc().ToString();

            _clientRepository.AddClient(addClientDto);

            return addClientDto;
        }
        public async Task<ClientDto> GetClientById(int id)
        {
            var client = await _clientRepository.GetClientById(id);
            var clientDto = _mapper.Map<ClientDto>(client);
            return clientDto;
        }
        public async Task<ClientDto> GetClientByGlobalId(Guid id)
        {
            var client = await _clientRepository.GetClientByGlobalId(id);
            var clientDto = _mapper.Map<ClientDto>(client);
            return clientDto;
        }

        public async Task<ClientDto> GetClientByEmail(string email)
        {
            var client = await _clientRepository.GetClientByEmail(email);
            var clientDto = _mapper.Map<ClientDto>(client);
            return clientDto;
        }
    }
}
