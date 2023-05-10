using System.ComponentModel.DataAnnotations;

namespace Levi9.POS.WebApi.Request.DocumentRequest
{
    public class CreateDocumentRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ClientId must be a positive integer.")]
        public int ClientId { get; set; }
        [Required]
        [RegularExpression("^(INVOICE|RECEIPTS|PURCHASE)$", ErrorMessage = "The value of Document type can be: INVOICE, RECEIPTS, or PURCHASE.")]
        public string DocumentType { get; set; }
        [Required]
        public List<CreateDocumentItemRequest> Items { get; set; }
    }
}
