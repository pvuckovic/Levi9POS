namespace Levi9.POS.WebApi.Response.DocumentResponse
{
    public class GetByIdDocumentResponse
    {
        public int Id { get; set; }
        public Guid GlobalId { get; set; }
        public int ClientId { get; set; }
        public string LastUpdate { get; set; }
        public string DocumentType { get; set; }
        public string CreationDay { get; set; }
        public List<GetByIdDocumentItemResponse> Items { get; set; }
    }
}
