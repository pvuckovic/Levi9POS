using Levi9.POS.Domain.DTOs.ClientDTOs;

namespace Levi9.POS.Domain.Common.IClient
{
    public interface IClientService
    {
        Task<ClientDto> AddClient(ClientDto client);
        Task<ClientDto> GetClientById(int id);
        Task<ClientDto> GetClientByGlobalId(Guid id);
        Task<ClientDto> GetClientByEmail(string email);
        Task<UpdateClientDto> UpdateClient(UpdateClientDto client);
        bool CheckEmailExist(string email);

    }
}
