using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levi9.POS.Domain.DTOs
{
    public class CreateDocumentItemDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public string Currency { get; set; }
        public string LastUpdate { get; set; }
    }
}
