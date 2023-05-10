using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DBContext;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Models;

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

        public AddClientDto AddClient(AddClientDto client)
        {
            Client clientMap = _mapper.Map<Client>(client);

            _dbContext.Clients.AddAsync(clientMap);
            _dbContext.SaveChangesAsync();

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
            var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Email == email);

            return client;
        }


    }
}
