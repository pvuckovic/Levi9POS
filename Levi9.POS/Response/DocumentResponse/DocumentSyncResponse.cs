using Levi9.POS.WebApi.Request.DocumentRequest;
using System.ComponentModel.DataAnnotations;

namespace Levi9.POS.WebApi.Response.DocumentResponse
{
    public class DocumentSyncResponse
    {
        public Guid GlobalId { get; set; }
        public Guid ClientId { get; set; }
        public string DocumentType { get; set; }
        public List<DocumentItemSyncResponse> Items { get; set; }
    }
}
