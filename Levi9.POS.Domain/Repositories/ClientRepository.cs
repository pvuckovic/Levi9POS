using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

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

        public async Task<bool> DoesClientExist(int clientId)
        {
            var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientId);
            return client != null;
        }
    }
}
