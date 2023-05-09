using System.ComponentModel.DataAnnotations;

namespace Levi9.POS.WebApi.Request
{
    public class DocumentRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ClientId must be a positive integer.")]
        public int ClientId { get; set; }
        [Required]
        [RegularExpression("^(INVOICE|RECEIPTS|PURCHASE)$", ErrorMessage = "The document type must be INVOICE, RECEIPTS, or PURCHASE.")]
        public string DocumentType { get; set; }
        [Required]
        public List<DocumentItemRequest> Items { get; set; }
    }
}
