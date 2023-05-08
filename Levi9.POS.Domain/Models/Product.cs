using System.ComponentModel.DataAnnotations;

namespace Levi9.POS.Domain.Models
{
    public class Product
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]  
        public Guid GlobalId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string ProductImageUrl { get; set; }= string.Empty;
        [Required]
        [Range(0, int.MaxValue)]
        public int AvailableQuantity { get; set; }
        [Required]
        [StringLength(18, MinimumLength = 18)]
        public string LastUpdate { get; set; }= string.Empty;
        [Required]
        [Range(0, float.MaxValue)]
        public float Price { get; set; }
        public List<ProductDocument> ProductDocuments { get; set; } = new List<ProductDocument>();
    }
}
