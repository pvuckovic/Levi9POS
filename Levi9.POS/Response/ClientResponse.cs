using System.ComponentModel.DataAnnotations;

namespace Levi9.POS.WebApi.Response
{
    public class ClientResponse
    {
        public int Id { get; set; }
        public Guid GlobalId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; } 
        public string Phone { get; set; } 
        public string LastUpdate { get; set; } 
    }
}
