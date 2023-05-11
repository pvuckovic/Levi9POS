using AutoMapper;
using Levi9.POS.Domain.Common.IClient;
using Levi9.POS.Domain.DTOs.ClientDTOs;
using Levi9.POS.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Levi9.POS.Domain.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly DataBaseContext _dbContext;
        private readonly IMapper _mapper;

        public ClientRepository(DataBaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ClientDto AddClient(ClientDto client)
        {
            Client clientMap = _mapper.Map<Client>(client);

            _dbContext.Clients.Add(clientMap);
            _dbContext.SaveChanges();

            return client;
        }
        public async Task<Client> GetClientByGlobalId(Guid globalId)
        {
            return await _dbContext.Clients.FirstOrDefaultAsync(c => c.GlobalId == globalId);
        }

        public async Task<Client> GetClientById(int id)
        {
            return await _dbContext.Clients.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Client> GetClientByEmail(string email)
        {
            return await _dbContext.Clients.FirstOrDefaultAsync(c => c.Email == email);
        }
        public async Task<bool> DoesClientExist(int clientId)
        {
            var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientId);
            return client != null;
        }
        public bool CheckClientExist(int clientId)
        {
            return _dbContext.Clients.Any(c => c.Id == clientId);
        }
        public bool CheckEmailExist(string email)
        {
            return _dbContext.Clients.Any(c => c.Email == email);
        }
        public async Task<Client> UpdateClient(Client client)
        {
            var clientExists = CheckClientExist(client.Id);

            if (clientExists)
            {
                _dbContext.Attach(client);
                _dbContext.Entry(client).Property(x => x.Name).IsModified = true;
                _dbContext.Entry(client).Property(x => x.Address).IsModified = true;
                _dbContext.Entry(client).Property(x => x.Phone).IsModified = true;
                _dbContext.Entry(client).Property(x => x.Email).IsModified = true;
                await _dbContext.SaveChangesAsync();
                return client;
            }
            return null;
        }
    }
}
