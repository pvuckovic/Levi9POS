namespace Levi9.POS.WebApi.Responses
{
    public class ProductResponse
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
