using System.ComponentModel.DataAnnotations;

namespace Levi9.POS.WebApi.Request
{
    public class ClientLogin
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
