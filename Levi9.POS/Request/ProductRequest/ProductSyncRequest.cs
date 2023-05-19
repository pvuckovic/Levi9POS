using System.ComponentModel.DataAnnotations;

namespace Levi9.POS.WebApi.Request.ProductRequest
{
    public class ProductSyncRequest
    {
        [Required]
        public Guid GlobalId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ProductImageUrl { get; set; }
        [Required]
        public int AvailableQuantity { get; set; }
        [Required]
        public string LastUpdate { get; set; }
        [Required]
        [Range(0, float.MaxValue)]
        public float Price { get; set; }
    }
}
