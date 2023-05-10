namespace Levi9.POS.Domain.DTOs.ProductDTOs
{
    public class ProductSearchRequestDTO
    {
        public int Page { get; set; }
        public string Name { get; set; }
        public string OrderBy { get; set; }
        public string Direction { get; set; }
    }
}
