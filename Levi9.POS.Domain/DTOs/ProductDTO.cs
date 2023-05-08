namespace Levi9.POS.Domain.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public Guid GlobalId { get; set; }
        public string Name { get; set; }
        public string ProductImageUrl { get; set; }
        public int AvailableQuantity { get; set; }
        public string LastUpdate { get; set; } = string.Empty;
        public float Price { get; set; }
    }
}
