using Levi9.POS.Domain.DTOs;

namespace Levi9.POS.WebApi.Response
{
    public class ProductSearchResponse
    {
        public IEnumerable<ProductResponse> Items { get; set; }
        public int Page { get; set; }
    }
}
