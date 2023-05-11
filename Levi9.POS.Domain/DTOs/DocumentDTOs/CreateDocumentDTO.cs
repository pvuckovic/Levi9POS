using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levi9.POS.Domain.DTOs.DocumentDTOs
{
    public class CreateDocumentDTO
    {
        public int Id { get; set; }
        public Guid GlobalId { get; set; }
        public int ClientId { get; set; }
        public string LastUpdate { get; set; }
        public string DocumentType { get; set; }
        public string CreationDay { get; set; }
        public List<CreateDocumentItemDTO> DocumentItems { get; set; }
    }
}
