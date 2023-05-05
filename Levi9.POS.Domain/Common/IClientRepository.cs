using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levi9.POS.Domain.Common
{
    public interface IClientRepository
    {
        Task<Client> AddClient(Client client);
        
    }
}
