namespace Levi9.POS.Domain.DTOs
{
    public class ProductSearchRequestDTO
    {
        public int Page { get; set; } = 1;
        public string Name { get; set; }
        public string OrderBy { get; set; }
        public string Direction { get; set; }
    }
}
