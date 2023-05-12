using System.ComponentModel.DataAnnotations;

namespace Levi9.POS.WebApi.Request.ClientRequests
{
    public class ClientUpdate
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public Guid GlobalId { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required, StringLength(255)]
        public string Address { get; set; }
        [Required, StringLength(150)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required, StringLength(50)]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong phone number")]
        public string Phone { get; set; }
    }
}
