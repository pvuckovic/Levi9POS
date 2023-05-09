using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levi9.POS.Domain.DTOs
{
    public class ProductSearchRequestDTO
    {
        public int Page { get; set; } = 1;
        public string Name { get; set; }
        public string OrderBy { get; set; } = "name";
        public string Direction { get; set; } = "asc";
    }
}
