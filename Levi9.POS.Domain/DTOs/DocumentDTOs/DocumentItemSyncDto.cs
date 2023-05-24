namespace Levi9.POS.Domain.DTOs.DocumentDTOs
{
    public class DocumentItemSyncDto
    {
        public string Name { get; set; }
        public Guid ProductId { get; set; }
        public float Price { get; set; }
        public string Currency { get; set; }
        public int Quantity { get; set; }
    }
}