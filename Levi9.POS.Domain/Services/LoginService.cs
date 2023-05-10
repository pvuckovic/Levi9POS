using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Helpers;
using Levi9.POS.Domain.Models;

namespace Levi9.POS.Domain.Services
{
    public class LoginService : ILoginService
    {
        private readonly IClientService  _clientService;
        private readonly IMapper _mapper;


        public LoginService(IClientService clientService, IMapper mapper)
        {
            _clientService = clientService;
            _mapper = mapper;
        }

        public async Task<bool> ValidateClient(ClientLogin clientLogin)
        {
            ClientDto client = await _clientService.GetClientByEmail(clientLogin.Email);
            if (client == null)
            {
                return false;
            }
            Client clientMap = _mapper.Map<Client>(client);

            return AuthenticationHelper.Validate(clientLogin.Password, clientMap.Salt, clientMap.PasswordHash);
        }
    }
}
