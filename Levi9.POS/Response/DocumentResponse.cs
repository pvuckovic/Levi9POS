namespace Levi9.POS.WebApi.Response
{
    public class DocumentResponse
    {
        public int Id { get; set; }
        public Guid GlobalId { get; set; }
        public int ClientId { get; set; }
        public string LastUpdate { get; set; }
        public string DocumetType { get; set; }
        public string CreationDay { get; set; }
        public List<DocumentItemResponse> Items { get; set; }
    }
}
