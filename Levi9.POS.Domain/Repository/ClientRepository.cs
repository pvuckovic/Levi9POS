using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DBContext;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levi9.POS.Domain.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly DataBaseContext _dbContext;

        public ClientRepository(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Client> AddClient(Client client)
        {
            await _dbContext.Clients.AddAsync(client);
            await _dbContext.SaveChangesAsync();
            return client;
        }
        
    }
}
