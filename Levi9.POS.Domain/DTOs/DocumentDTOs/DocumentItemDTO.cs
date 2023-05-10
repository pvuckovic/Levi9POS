namespace Levi9.POS.Domain.DTOs.DocumentDTOs
{
    public class DocumentItemDTO
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public string Currency { get; set; }
        public string LastUpdate { get; set; }
    }
}
