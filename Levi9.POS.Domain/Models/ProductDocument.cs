using System.ComponentModel.DataAnnotations;

namespace Levi9.POS.Domain.Models
{
    public class ProductDocument
    {
        public int ProductId { get; set; }
        public int DocumentId { get; set; }
        [Required]
        [Range(1,int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        [Range(0, float.MaxValue)]
        public float Price { get; set; }
        [Required]
        [StringLength(3,MinimumLength = 3)]
        public string Currency { get; set; } =string.Empty;



        public Product Product { get; set; } = null!;
        public Document Document { get; set; } = null!;


    }
}
