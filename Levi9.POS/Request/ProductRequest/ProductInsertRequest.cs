namespace Levi9.POS.WebApi.Request.ProductRequest
{
    public class ProductInsertRequest
    {
        public Guid GlobalId { get; set; }
        public string Name { get; set; }
        public string ProductImageUrl { get; set; }
        public int AvailableQuantity { get; set; }
        public string LastUpdate { get; set; }
        public float Price { get; set; }
    }
}
