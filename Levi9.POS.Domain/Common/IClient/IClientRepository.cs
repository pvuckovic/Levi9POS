using Levi9.POS.Domain.DTOs.ClientDTOs;
using Levi9.POS.Domain.Models;

namespace Levi9.POS.Domain.Common.IClient
{
    public interface IClientRepository
    {
        ClientDto AddClient(ClientDto client);
        Task<Client> GetClientById(int id);
        Task<Client> GetClientByGlobalId(Guid id);
        Task<Client> GetClientByEmail(string email);
        Task<Client> UpdateClient(Client client);
        Task<bool> DoesClientExist(int clientId);
        bool CheckClientExist(int clientId);
        bool CheckEmailExist(string email);
        Task<IEnumerable<Client>> GetClientsByLastUpdate(string lastUpdate);
        Task<string> InsertClientAsync(Client client);
        Task<string> UpdateClientAsync(Client client);
        Task<List<Client>> GetClientsWithLastUpdateGreaterThan(string syncLastUpdate, List<string> lastUpdates);

    }
}
