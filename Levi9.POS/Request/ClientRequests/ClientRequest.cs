using System.ComponentModel.DataAnnotations;

namespace Levi9.POS.WebApi.Request.ClientRequests
{
    public class ClientRequest
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [StringLength(150)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong mobile")]
        public string Phone { get; set; }
        [Required]
        [StringLength(50)]
        public string Password { get; set; }
    }
}
