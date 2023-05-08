using System.ComponentModel.DataAnnotations;

namespace Levi9.POS.Domain.Models
{
    public class Client
    {
        [Required]
        [Key]
        public int Id { get; set; }
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
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        public string Phone { get; set; }
        [Required]
        [StringLength(18, MinimumLength = 18)]
        public string LastUpdate { get; set; }
        public List<Document> Documents { get; set; }
    }
}
