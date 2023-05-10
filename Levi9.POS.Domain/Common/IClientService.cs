using Levi9.POS.Domain.DTOs;

namespace Levi9.POS.Domain.Common
{
    public interface IClientService
    {
        Task<ClientDto> AddClient(ClientDto client);
        Task<ClientDto> GetClientById(int id);
        Task<ClientDto> GetClientByGlobalId(Guid id);
    }
}
