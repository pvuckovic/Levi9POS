using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levi9.POS.Domain.DTOs.ClientDTOs
{
    public class ClientsSyncDto
    {
        public List<ClientSyncDto> Clients { get; set; }
        public string? LastUpdate { get; set; }
    }
}
