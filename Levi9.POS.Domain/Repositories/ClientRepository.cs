using AutoMapper;
using Levi9.POS.Domain.Common.IClient;
using Levi9.POS.Domain.DTOs.ClientDTOs;
using Levi9.POS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Levi9.POS.Domain.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly DataBaseContext _dbContext;
        private readonly ILogger<ClientRepository> _logger;
        private readonly IMapper _mapper;

        public ClientRepository(DataBaseContext dbContext, ILogger<ClientRepository> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public ClientDto AddClient(ClientDto client)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientRepository. Timestamp: {Timestamp}.", nameof(AddClient), DateTime.UtcNow);
            Client clientMap = _mapper.Map<Client>(client);

            _dbContext.Clients.Add(clientMap);
            _dbContext.SaveChanges();
            _logger.LogInformation("Retrieving new client in {FunctionName} of ClientRepository. Timestamp: {Timestamp}.", nameof(AddClient), DateTime.UtcNow);
            return client;
        }
        public async Task<Client> GetClientByGlobalId(Guid globalId)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientRepository. Timestamp: {Timestamp}.", nameof(GetClientByGlobalId), DateTime.UtcNow);
            var result = await _dbContext.Clients.FirstOrDefaultAsync(c => c.GlobalId == globalId);
            _logger.LogInformation("Retrieving client with GlobalId: {GlobalId} in {FunctionName} of ClientRepository. Timestamp: {Timestamp}.", globalId, nameof(GetClientByGlobalId), DateTime.UtcNow);
            return result;
        }

        public async Task<Client> GetClientById(int id)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientRepository. Timestamp: {Timestamp}.", nameof(GetClientById), DateTime.UtcNow);
            var result = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Id == id);
            _logger.LogInformation("Retrieving client with ID: {Id} in {FunctionName} of ClientRepository. Timestamp: {Timestamp}.", id, nameof(GetClientById), DateTime.UtcNow);
            return result;
        }
        public async Task<Client> GetClientByEmail(string email)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientRepository. Timestamp: {Timestamp}.", nameof(GetClientByEmail), DateTime.UtcNow);
            var result = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Email == email);
            _logger.LogInformation("Retrieving client with Email: {Email} in {FunctionName} of ClientRepository. Timestamp: {Timestamp}.", email, nameof(GetClientByEmail), DateTime.UtcNow);
            return result;
        }
        public async Task<bool> DoesClientExist(int clientId)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientRepository. Timestamp: {Timestamp}.", nameof(DoesClientExist), DateTime.UtcNow);
            var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientId);
            _logger.LogInformation("Retrieving confirmation of client existence with ID: {Id} existence in {FunctionName} of ClientRepository. Timestamp: {Timestamp}.", clientId, nameof(DoesClientExist), DateTime.UtcNow);
            return client != null;
        }
        public bool CheckClientExist(int clientId)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientRepository. Timestamp: {Timestamp}.", nameof(CheckClientExist), DateTime.UtcNow);
            var result = _dbContext.Clients.Any(c => c.Id == clientId);
            _logger.LogInformation("Retrieving confirmation of client existence with ID: {Id} existence in {FunctionName} of ClientRepository. Timestamp: {Timestamp}.", clientId, nameof(CheckClientExist), DateTime.UtcNow);
            return result;
        }
        public bool CheckEmailExist(string email)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientRepository. Timestamp: {Timestamp}.", nameof(CheckEmailExist), DateTime.UtcNow);
            var result = _dbContext.Clients.Any(c => c.Email == email);
            _logger.LogInformation("Retrieving confirmation of email: {Email} existence in {FunctionName} of ClientRepository. Timestamp: {Timestamp}.", email, nameof(CheckEmailExist), DateTime.UtcNow);
            return result;
        }
        public async Task<Client> UpdateClient(Client client)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientRepository. Timestamp: {Timestamp}.", nameof(UpdateClient), DateTime.UtcNow);
            var clientExists = CheckClientExist(client.Id);

            if (clientExists)
            {
                _dbContext.Attach(client);
                _dbContext.Entry(client).Property(x => x.Name).IsModified = true;
                _dbContext.Entry(client).Property(x => x.Address).IsModified = true;
                _dbContext.Entry(client).Property(x => x.Phone).IsModified = true;
                _dbContext.Entry(client).Property(x => x.Email).IsModified = true;
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Client updated successfully in {FunctionName} of ClientRepository. Timestamp: {Timestamp}.", nameof(UpdateClient), DateTime.UtcNow);
                return client;
            }
            _logger.LogError("Failed to update client in {FunctionName} of ClientRepository. Timestamp: {Timestamp}.", nameof(UpdateClient), DateTime.UtcNow);
            return null;
        }
    }
}
