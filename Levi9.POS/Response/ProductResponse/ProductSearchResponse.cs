namespace Levi9.POS.WebApi.Response.ProductResponse
{
    public class ProductSearchResponse
    {
        public IEnumerable<ProductResponse> Items { get; set; }
        public int Page { get; set; }
    }
}
