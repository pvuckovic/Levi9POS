using System.ComponentModel.DataAnnotations;

namespace Levi9.POS.WebApi.Response
{
    public class AddClientResponse
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public Guid GlobalId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(255)]
        public string Address { get; set; } = string.Empty;
        [Required]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string Phone { get; set; } = string.Empty;
        [Required]
        [StringLength(18, MinimumLength = 18)]
        public string LastUpdate { get; set; } = string.Empty;

    }
}
