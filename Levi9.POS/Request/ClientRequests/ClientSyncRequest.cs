using System.ComponentModel.DataAnnotations;

namespace Levi9.POS.WebApi.Request.ClientRequests
{
    public class ClientSyncRequest
    {
        [Required]
        public Guid GlobalId { get; set; }

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
        public string Phone { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [StringLength(18, MinimumLength = 18)]
        public string LastUpdate { get; set; }

        [Required]
        [StringLength(50)]
        public string Salt { get; set; }
    }
}
